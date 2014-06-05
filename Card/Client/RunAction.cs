using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Client
{
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
            Card.CardBasicInfo card = Card.CardUtility.GetCardInfoBySN(CardSn);
            List<String> ActionCodeLst = new List<string>();
            switch (card.CardType)
            {
                case CardBasicInfo.CardTypeEnum.法术:
                    ActionCodeLst.Add(UseAbility(CardSn));
                    //初始化 Buff效果等等
                    Card.AbilityCard ablity = (Card.AbilityCard)CardUtility.GetCardInfoBySN(CardSn);
                    //连击效果的法术修改
                    if (game.MySelf.RoleInfo.IsCombit && (!String.IsNullOrEmpty(card.连击效果)))
                    {
                        ablity = (Card.AbilityCard)CardUtility.GetCardInfoBySN(card.连击效果);
                    }
                    ablity.CardAbility.Init();
                    var ResultArg = game.UseAbility(ablity, ConvertPosDirect);
                    if (ResultArg.Count != 0)
                    {
                        ActionCodeLst.AddRange(ResultArg);
                        //英雄技能等的时候，不算[本方施法] 
                        if (CardSn.Substring(1, 1) == "0") ActionCodeLst.AddRange(game.MySelf.RoleInfo.BattleField.触发事件(new Card.CardUtility.全局事件(){ 事件类型= CardUtility.事件类型列表.施法},game));
                    }
                    else
                    {
                        ActionCodeLst.Clear();
                    }
                    break;
                case CardBasicInfo.CardTypeEnum.随从:
                    int MinionPos = 1;
                    if (game.MySelf.RoleInfo.BattleField.MinionCount != 0) MinionPos = GetPutPos(game);
                    if (MinionPos != -1)
                    {
                        ActionCodeLst.Add(UseMinion(CardSn, MinionPos));
                        var minion = (Card.MinionCard)card;
                        //初始化
                        minion.Init();
                        //必须在放入之前做得原因是，被放入的随从不能被触发这个事件
                        ActionCodeLst.AddRange(game.MySelf.RoleInfo.BattleField.触发事件(
                            new Card.CardUtility.全局事件() { 事件类型 = CardUtility.事件类型列表.召唤, 附加信息 = minion.种族.ToString() }, game));
                        switch (minion.战吼类型)
                        {
                            case MinionCard.战吼类型列表.默认:
                                game.MySelf.RoleInfo.BattleField.PutToBattle(MinionPos, minion);
                                ActionCodeLst.AddRange(minion.发动战吼(game));
                                break;
                            case MinionCard.战吼类型列表.抢先:
                                //战吼中，其他 系列的法术效果
                                foreach (var result in minion.发动战吼(game))
                                {
                                    var resultArray = result.Split(CardUtility.strSplitMark.ToCharArray());
                                    if (int.Parse(resultArray[2]) < MinionPos)
                                    {
                                        ActionCodeLst.Add(result);
                                    }
                                    else
                                    {
                                        ActionCodeLst.Add(resultArray[0] + CardUtility.strSplitMark + resultArray[1] + CardUtility.strSplitMark +
                                                           (int.Parse(resultArray[2]) + 1).ToString() + CardUtility.strSplitMark + resultArray[3]);
                                    }
                                }
                                game.MySelf.RoleInfo.BattleField.PutToBattle(MinionPos, minion);
                                break;
                            case MinionCard.战吼类型列表.相邻:
                            case MinionCard.战吼类型列表.自身:
                                game.MySelf.RoleInfo.BattleField.PutToBattle(MinionPos, minion);
                                game.MySelf.RoleInfo.BattleField.发动战吼(MinionPos);
                                break;
                            default:
                                break;
                        }
                        game.MySelf.RoleInfo.BattleField.ResetBuff();
                    }
                    else
                    {
                        ActionCodeLst.Clear();
                    }
                    break;
                case CardBasicInfo.CardTypeEnum.武器:
                    ActionCodeLst.Add(UseWeapon(CardSn));
                    game.MySelf.RoleInfo.Weapon = (Card.WeaponCard)card;
                    break;
                case CardBasicInfo.CardTypeEnum.奥秘:
                    ActionCodeLst.Add(UseSecret(CardSn));
                    game.MySelf.奥秘列表.Add((Card.SecretCard)card);
                    game.MySelf.RoleInfo.SecretCount = game.MySelf.奥秘列表.Count;
                    break;
                default:
                    break;
            }
            //连击启动(法术的时候是修改法术内容)
            if (card.CardType != CardBasicInfo.CardTypeEnum.法术 && game.MySelf.RoleInfo.IsCombit)
            {
                if (!String.IsNullOrEmpty(card.连击效果))
                {
                    //初始化 Buff效果等等
                    Card.AbilityCard ablity = (Card.AbilityCard)CardUtility.GetCardInfoBySN(card.连击效果);
                    ablity.CardAbility.Init();
                    var ResultArg = game.UseAbility(ablity, ConvertPosDirect);
                    if (ResultArg.Count != 0)
                    {
                        ActionCodeLst.AddRange(ResultArg);
                        //英雄技能等的时候，不算[本方施法] 
                        if (CardSn.Substring(1, 1) == "0") ActionCodeLst.AddRange(game.MySelf.RoleInfo.BattleField.触发事件(new Card.CardUtility.全局事件() { 事件类型 = CardUtility.事件类型列表.施法 }, game));
                    }
                }
            }
            if (ActionCodeLst.Count != 0) game.MySelf.RoleInfo.IsCombit = true;
            return ActionCodeLst;
        }
        /// <summary>
        /// 使用武器
        /// </summary>
        /// <param name="CardSn">卡牌号码</param>
        /// <returns></returns>
        public static String UseWeapon(String CardSn)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strWeapon + CardUtility.strSplitMark + CardSn;
            return actionCode;
        }
        /// <summary>
        /// 使用奥秘
        /// </summary>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static String UseSecret(String CardSn)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strSecret + CardUtility.strSplitMark + CardSn;
            return actionCode;
        }
        /// <summary>
        /// 使用随从
        /// </summary>
        /// <param name="CardSn">卡牌号码</param>
        /// <param name="Position"></param>
        /// <returns></returns>
        public static String UseMinion(String CardSn, int Position)
        {
            String actionCode = String.Empty;
            //MINION#M000001#1
            actionCode = ActionCode.strMinion + CardUtility.strSplitMark + CardSn + CardUtility.strSplitMark + Position.ToString("D1");
            return actionCode;
        }
        /// <summary>
        /// 使用魔法
        /// </summary>
        /// <param name="CardSn">卡牌号码</param>
        /// <returns></returns>
        public static String UseAbility(String CardSn)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strAbility + CardUtility.strSplitMark + CardSn;
            return actionCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="MyPos"></param>
        /// <param name="YourPos"></param>
        /// <returns></returns>
        public static List<String> Fight(GameManager game, int MyPos, int YourPos)
        {
            String actionCode = String.Empty;
            //FIGHT#1#2
            actionCode = ActionCode.strFight + CardUtility.strSplitMark + MyPos + CardUtility.strSplitMark + YourPos;
            List<String> ActionCodeLst = new List<string>();
            ActionCodeLst.Add(actionCode);
            ActionCodeLst.AddRange(game.Fight(MyPos, YourPos, false));
            return ActionCodeLst;
        }
        #endregion
    }
}
