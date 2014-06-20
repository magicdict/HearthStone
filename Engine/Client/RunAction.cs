using Engine.Card;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Client
{
    /// <summary>
    /// 执行
    /// </summary>
    public static class RunAction
    {
        /// <summary>
        /// 获目标位置
        /// </summary>
        public static CardUtility.delegateGetPutPos GetPutPos;
        #region"开始动作"
        /// <summary>
        /// 开始一个动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="CardSn"></param>
        /// <param name="ConvertPosDirect">亡语的时候，需要倒置方向</param>
        /// <returns></returns>
        public static List<String> StartAction(GameManager game, String CardSn, Boolean ConvertPosDirect = false)
        {
            //清除事件池，注意，事件将在动作结束后整体结算
            game.事件处理组件.事件池.Clear();
            Engine.Card.CardBasicInfo card = Engine.Utility.CardUtility.GetCardInfoBySN(CardSn);
            List<String> ActionCodeLst = new List<string>();
            switch (card.CardType)
            {
                case CardBasicInfo.CardTypeEnum.法术:
                    ActionCodeLst.Add(ActionCode.strAbility + CardUtility.strSplitMark + CardSn);
                    //初始化 Buff效果等等
                    Engine.Card.AbilityCard ablity = (Engine.Card.AbilityCard)CardUtility.GetCardInfoBySN(CardSn);
                    ablity.Init();
                    var ResultArg = ablity.UseAbility(game, ConvertPosDirect);
                    if (ResultArg.Count != 0)
                    {
                        ActionCodeLst.AddRange(ResultArg);
                        //英雄技能等的时候，不算[本方施法] 
                        if (CardSn.Substring(1, 1) == Engine.Card.AbilityCard.原生法术)
                            game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                            {
                                触发事件类型 = CardUtility.事件类型列表.施法,
                                触发位置 = game.HostInfo.战场位置
                            });
                    }
                    else
                    {
                        ActionCodeLst.Clear();
                    }
                    break;
                case CardBasicInfo.CardTypeEnum.随从:
                    int MinionPos = 1;
                    if (game.HostInfo.BattleField.MinionCount != 0) MinionPos = GetPutPos(game);
                    if (MinionPos != -1)
                    {
                        ActionCodeLst.Add(ActionCode.strMinion + CardUtility.strSplitMark + CardSn + CardUtility.strSplitMark + MinionPos.ToString("D1"));
                        var minion = (Engine.Card.MinionCard)card;
                        //初始化
                        minion.Init();
                        //必须在放入之前做得原因是，被放入的随从不能被触发这个事件
                        game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = new CardUtility.TargetPosition()
                            {
                                Postion = MinionPos,
                                本方对方标识 = true
                            }
                        });
                        switch (minion.战吼类型)
                        {
                            case MinionCard.战吼类型列表.默认:
                                game.HostInfo.BattleField.PutToBattle(MinionPos, minion);
                                ActionCodeLst.AddRange(minion.发动战吼(game));
                                break;
                            case MinionCard.战吼类型列表.抢先:
                                //战吼中，其他系列的法术效果 例如其他鱼人获得XX效果
                                //战吼中，友方系列的法术效果 例如友方随从获得XX效果
                                foreach (var result in minion.发动战吼(game))
                                {
                                    var resultArray = result.Split(CardUtility.strSplitMark.ToCharArray());
                                    if (resultArray.Length == 1 || int.Parse(resultArray[2]) < MinionPos)
                                    {
                                        //SETTLE的时候为1
                                        ActionCodeLst.Add(result);
                                    }
                                    else
                                    {
                                        //位置的调整，后面的随从的位置需要调整
                                        ActionCodeLst.Add(resultArray[0] + CardUtility.strSplitMark + resultArray[1] + CardUtility.strSplitMark +
                                        (int.Parse(resultArray[2]) + 1).ToString() + CardUtility.strSplitMark + resultArray[3]);
                                    }
                                }
                                game.HostInfo.BattleField.PutToBattle(MinionPos, minion);
                                break;
                            case MinionCard.战吼类型列表.相邻:
                            case MinionCard.战吼类型列表.自身:
                                game.HostInfo.BattleField.PutToBattle(MinionPos, minion);
                                game.HostInfo.BattleField.发动战吼(MinionPos, game);
                                break;
                            default:
                                break;
                        }
                        game.HostInfo.BattleField.ResetBuff();
                    }
                    else
                    {
                        ActionCodeLst.Clear();
                    }
                    break;
                case CardBasicInfo.CardTypeEnum.武器:
                    ActionCodeLst.Add(ActionCode.strWeapon + CardUtility.strSplitMark + CardSn);
                    game.HostInfo.Weapon = (Engine.Card.WeaponCard)card;
                    break;
                case CardBasicInfo.CardTypeEnum.奥秘:
                    ActionCodeLst.Add(ActionCode.strSecret + CardUtility.strSplitMark + CardSn);
                    game.HostSelfInfo.奥秘列表.Add((Engine.Card.SecretCard)card);
                    game.HostInfo.SecretCount = game.HostSelfInfo.奥秘列表.Count;
                    break;
                default:
                    break;
            }
            //随从卡牌的连击效果启动
            if (card.CardType != CardBasicInfo.CardTypeEnum.法术 && game.HostInfo.连击状态)
            {
                if (!String.IsNullOrEmpty(card.连击效果))
                {
                    //初始化 Buff效果等等
                    Engine.Card.AbilityCard ablity = (Engine.Card.AbilityCard)CardUtility.GetCardInfoBySN(card.连击效果);
                    ablity.Init();
                    var ResultArg = ablity.UseAbility(game, ConvertPosDirect);
                    if (ResultArg.Count != 0)
                    {
                        ActionCodeLst.AddRange(ResultArg);
                        //英雄技能等的时候，不算[本方施法] 
                        if (CardSn.Substring(1, 1) == Engine.Card.AbilityCard.原生法术)
                            game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                            {
                                触发事件类型 = CardUtility.事件类型列表.施法,
                                触发位置 = game.HostInfo.战场位置
                            });
                    }
                }
            }
            if (ActionCodeLst.Count != 0)
            {
                game.HostInfo.连击状态 = true;
                ActionCodeLst.AddRange(game.事件处理组件.事件处理(game));
            }
            return ActionCodeLst;
        }

        /// <summary>
        /// 战斗
        /// </summary>
        /// <param name="game"></param>
        /// <param name="MyPos"></param>
        /// <param name="YourPos"></param>
        /// <returns></returns>
        public static List<String> Fight(GameManager game, int MyPos, int YourPos)
        {
            game.事件处理组件.事件池.Clear();
            //FIGHT#1#2
            String actionCode = ActionCode.strFight + CardUtility.strSplitMark + MyPos + CardUtility.strSplitMark + YourPos;
            List<String> ActionCodeLst = new List<string>();
            ActionCodeLst.Add(actionCode);
            ActionCodeLst.AddRange(FightHandler.Fight(MyPos, YourPos, game, false));
            ActionCodeLst.AddRange(game.事件处理组件.事件处理(game));
            return ActionCodeLst;
        }
        #endregion
    }
}
