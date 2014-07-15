using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Action
{
    public static class UseMinionAction
    {
        #region "CS架构"
        /// <summary>
        /// 获目标位置
        /// </summary>
        public static CardUtility.delegateGetMinionPos GetMinionPos;
        /// <summary>
        /// 随从卡牌
        /// </summary>
        /// <param name="game"></param>
        /// <param name="CardSn"></param>
        /// <param name="AIParm"></param>
        /// <param name="card"></param>
        /// <param name="ActionCodeLst"></param>
        /// <param name="PlayInfo"></param>
        public static void RunCS(ActionStatus game,
                               String CardSn,
                               CardBasicInfo card,
                               List<String> ActionCodeLst,
                               PublicInfo PlayInfo)
        {
            int MinionPos = 1;
            MinionCard minion = null;
            //Step1
            if (PlayInfo.BattleField.MinionCount != 0)
            {
                if (game.Interrupt.SessionData == null)
                {
                    MinionPos = GetMinionPos(game.AllRole.MyPublicInfo.BattleField);
                }
                else
                {
                    MinionPos = int.Parse(game.Interrupt.SessionData);
                }
            }
            //Step2
            if (MinionPos != -1)
            {
                ActionCodeLst.Add(ActionCode.strMinion + CardUtility.strSplitMark + CardSn + CardUtility.strSplitMark + MinionPos.ToString("D1"));
                minion = (Engine.Card.MinionCard)card;
                //初始化
                minion.初始化();
                //必须在放入之前做得原因是，被放入的随从不能被触发这个事件
                game.battleEvenetHandler.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.召唤,
                    触发位置 = new CardUtility.指定位置结构体()
                    {
                        位置 = MinionPos,
                        本方对方标识 = true
                    }
                });
            }
            //Step3
            if (minion != null)
            {
                switch (minion.战吼类型)
                {
                    case MinionCard.战吼类型枚举.默认:
                    case MinionCard.战吼类型枚举.相邻:
                    case MinionCard.战吼类型枚举.自身:
                        PlayInfo.BattleField.发动战吼(MinionPos, game);
                        PlayInfo.BattleField.PutToBattle(MinionPos, minion);
                        ActionCodeLst.AddRange(minion.发动战吼(game));
                        break;
                    case MinionCard.战吼类型枚举.抢先:
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
                        PlayInfo.BattleField.PutToBattle(MinionPos, minion);
                        break;
                    default:
                        break;
                }
                PlayInfo.BattleField.ResetBuff();
            }
        }
        #endregion

        #region "BS架构"
        /// <summary>
        /// 随从卡牌
        /// </summary>
        /// <param name="game"></param>
        /// <param name="CardSn"></param>
        public static void RunBS(ActionStatus game, String CardSn)
        {
            int MinionPos = -1;
            MinionCard minion = minion = (Engine.Card.MinionCard)CardUtility.GetCardInfoBySN(CardSn);
            //Step1
            if (game.Interrupt.Step == 1)
            {
                if (game.AllRole.MyPublicInfo.BattleField.MinionCount != 0)
                {
                    game.Interrupt.Step = 2;
                    game.Interrupt.ActionName = "MINIONPOSITION";
                    return;
                }
                MinionPos = 1;
                game.Interrupt.Step = 2;
            }
            //Step2
            if (game.Interrupt.Step == 2)
            {
                if (MinionPos == -1) MinionPos = int.Parse(game.Interrupt.SessionData);
                //初始化
                minion.初始化();
                //随从入场
                game.AllRole.MyPublicInfo.BattleField.PutToBattle(MinionPos, minion);
                //必须在放入之前做得原因是，被放入的随从不能被触发这个事件
                game.battleEvenetHandler.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                {
                    触发事件类型 = CardUtility.事件类型枚举.召唤,
                    触发位置 = new CardUtility.指定位置结构体()
                    {
                        位置 = MinionPos,
                        本方对方标识 = game.IsHost
                    }
                });
                game.Interrupt.Step = 3;
            }
            //Step3
            if (game.Interrupt.Step == 3 && !String.IsNullOrEmpty(minion.战吼效果))
            {
                SpellCard spell = (SpellCard)CardUtility.GetCardInfoBySN(minion.战吼效果);
                if (spell.FirstAbilityDefine.IsNeedTargetSelect)
                {
                    SelectUtility.SetTargetSelectEnable(spell.FirstAbilityDefine.MainAbilityDefine.AbliltyPosPicker, game);
                    game.Interrupt.ExternalInfo = SelectUtility.GetTargetList(game);
                    game.Interrupt.Step = 4;
                    game.Interrupt.ActionName = "BATTLECRYPOSITION";
                    return;
                }
                else
                {
                    //注意：发动战吼的方法自身或者相邻的类型，所以肯定不需要做目标选择
                    if (spell.FirstAbilityDefine.MainAbilityDefine.AbliltyPosPicker.EffictTargetSelectMode == CardUtility.目标选择模式枚举.相邻 ||
                        spell.FirstAbilityDefine.MainAbilityDefine.AbliltyPosPicker.EffictTargetSelectMode == CardUtility.目标选择模式枚举.继承)
                    {
                        game.AllRole.MyPublicInfo.BattleField.发动战吼(MinionPos, game);
                    }
                    else
                    {
                        game.Interrupt.Step = 4;
                    }
                }
            }
            if (game.Interrupt.Step == 4)
            {
                //法术位置信息包含在SessionData中，传递下去
                //这种类型的战吼，直接转换为施法
                //这里约定：战吼是无需抉择的
                game.Interrupt.ActionName = "RUNBATTLECRY";
                game.Interrupt.Step = 1;
                UseSpellAction.RunBS(game, minion.战吼效果);
            }
            game.Interrupt.Step = 99;
            game.Interrupt.ActionName = CardUtility.strOK;
        }
        #endregion

    }
}
