using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Card
{
    /// <summary>
    /// 奥秘卡牌
    /// </summary>
    [Serializable]
    public class SecretCard : CardBasicInfo
    {
        /// <summary>
        /// 触发条件类型
        /// </summary>
        public enum SecretCondition
        {
            对方召唤随从
        }
        public SecretCondition Condition = SecretCondition.对方召唤随从;
        /// <summary>
        /// 附加信息
        /// </summary>
        public String AdditionInfo = String.Empty;
        /// <summary>
        /// 是否已经触发
        /// </summary>
        public Boolean IsHitted = false;
        /// <summary>
        /// 奥秘是否命中
        /// </summary>
        /// <param name="SecretCardSN"></param>
        /// <param name="ActionCode"></param>
        /// <param name="HitMySelf">是否检查自己是否触发自己的奥秘</param>
        /// <returns></returns>
        public static Boolean IsSecretHit(String SecretCardSN, String ActionCode, Boolean HitMySelf)
        {
            //HitMySelf 
            //这里需要关注方向性问题
            SecretCard card = (SecretCard)CardUtility.GetCardInfoBySN(SecretCardSN);
            var actiontype = Card.Server.ActionCode.GetActionType(ActionCode);
            var actionField = ActionCode.Split(Card.CardUtility.strSplitMark.ToCharArray());
            Boolean IsHit = false;
            switch (card.Condition)
            {
                case SecretCondition.对方召唤随从:
                    if (HitMySelf)
                    {
                        //在自己的回合检查否触发自己的奥秘,如果是召唤系的
                        //SUMMON#YOU#M000001
                        //HitMySelf的时候，是YOU
                        if (actiontype == Server.ActionCode.ActionType.Summon && actionField[1] == CardUtility.strYou) IsHit = true;
                    }
                    else
                    {
                        //在别人的回合检查否触发自己的奥秘，是ME
                        if (actiontype == Server.ActionCode.ActionType.Summon && actionField[1] == CardUtility.strMe) IsHit = true;
                        if (actiontype == Server.ActionCode.ActionType.UseMinion) IsHit = true;
                    }
                    break;
                default:
                    break;
            }
            return IsHit;
        }
        /// <summary>
        /// 运行奥秘
        /// </summary>
        /// <param name="SecretCardSN"></param>
        /// <param name="ActionCode"></param>
        /// <param name="HitMySelf"></param>
        /// <returns></returns>
        public static List<String> RunSecretHit(String SecretCardSN, String ActionCode, Boolean HitMySelf, Card.Client.GameManager game)
        {
            List<String> ActionLst = new List<string>();
            SecretCard card = (SecretCard)CardUtility.GetCardInfoBySN(SecretCardSN);
            var actiontype = Card.Server.ActionCode.GetActionType(ActionCode);
            var actionField = ActionCode.Split(Card.CardUtility.strSplitMark.ToCharArray());
            ActionLst.Add(Card.Server.ActionCode.strHitSecret + CardUtility.strSplitMark + (HitMySelf ? CardUtility.strMe : CardUtility.strYou) +
                                                                CardUtility.strSplitMark + SecretCardSN);
            //HitMySelf 在自己的回合运行自己的奥秘
            switch (card.Condition)
            {
                case SecretCondition.对方召唤随从:
                    //如果是召唤系的
                    if (HitMySelf)
                    {
                        //在自己的回合运行自己的奥秘
                        //SUMMON#YOU#M000001#POS
                        //例如：亡语的时候可能召唤一个新的随从
                        game.YourInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1].实际生命值 = 1;
                        ActionLst.Add(Card.Server.ActionCode.strPoint + Card.CardUtility.strSplitMark + CardUtility.strYou + Card.CardUtility.strSplitMark +
                                    actionField[3] + Card.CardUtility.strSplitMark + "1/" + game.YourInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1].实际攻击力);
                    }
                    else
                    {
                        //在自己的回合运行别人的奥秘
                        if (actiontype == Server.ActionCode.ActionType.Summon)
                        {
                            //SUMMON#YOU#M000001#POS
                            game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1].实际生命值 = 1;
                            ActionLst.Add(Card.Server.ActionCode.strPoint + Card.CardUtility.strSplitMark + CardUtility.strMe + Card.CardUtility.strSplitMark +
                                    actionField[3] + Card.CardUtility.strSplitMark + "1/" + game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1].实际攻击力);
                        }
                        else
                        {
                            //MINION#M000001#1
                            game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actionField[2]) - 1].实际生命值 = 1;
                            ActionLst.Add(Card.Server.ActionCode.strPoint + Card.CardUtility.strSplitMark + CardUtility.strMe + Card.CardUtility.strSplitMark +
                                    actionField[2] + Card.CardUtility.strSplitMark + "1/" + game.MySelf.RoleInfo.BattleField.BattleMinions[int.Parse(actionField[2]) - 1].实际攻击力);
                        }
                    }
                    break;
                default:
                    break;
            }
            return ActionLst;
        }
    }
}
