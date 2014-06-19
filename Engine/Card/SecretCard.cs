using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Card
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
            var actiontype = Engine.Server.ActionCode.GetActionType(ActionCode);
            var actionField = ActionCode.Split(Engine.Utility.CardUtility.strSplitMark.ToCharArray());
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
        /// 奥秘计算
        /// </summary>
        /// <param name="actionlst"></param>
        /// <returns></returns>
        public static List<String> 奥秘计算(List<String> actionlst, GameManager game)
        {
            List<String> Result = new List<string>();
            //奥秘计算 START
            //本方（Fight也需要）
            if (game.MySelfInfo.奥秘列表.Count != 0)
            {
                //本方的行动触发本方奥秘的检查
                for (int i = 0; i < actionlst.Count; i++)
                {
                    foreach (var secret in game.MySelfInfo.奥秘列表)
                    {
                        if ((!secret.IsHitted) && Engine.Card.SecretCard.IsSecretHit(secret.序列号, actionlst[i], true))
                        {
                            //奥秘执行
                            Result.AddRange(Engine.Card.SecretCard.RunSecretHit(secret.序列号, actionlst[i], true, game));
                            secret.IsHitted = true;
                        }
                    }
                }
                //移除已经触发的奥秘
                game.MySelfInfo.清除命中奥秘();
            }
            //对方（Fight也需要）
            if (game.YourInfo.SecretCount != 0)
            {
                var HitCard = Engine.Client.ClientRequest.IsSecretHit(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, actionlst);
                if (!String.IsNullOrEmpty(HitCard))
                {
                    var HitCardList = HitCard.Split(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
                    foreach (var hitCard in HitCardList)
                    {
                        Result.AddRange(Engine.Card.SecretCard.RunSecretHit(hitCard.Split(Engine.Utility.CardUtility.strSplitDiffMark.ToCharArray())[0],
                                                                     hitCard.Split(Engine.Utility.CardUtility.strSplitDiffMark.ToCharArray())[1], false, game));
                        game.YourInfo.SecretCount--;
                    }
                }
            }
            //奥秘计算 END
            Result.AddRange(game.Settle());
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        public static void ReRunSecret(GameManager game, String[] actField)
        {
            if (actField[1] == CardUtility.strYou)
            {
                Engine.Card.SecretCard Hit = new SecretCard();
                foreach (var secret in game.MySelfInfo.奥秘列表)
                {
                    if (secret.序列号 == actField[2])
                    {
                        Hit = secret;
                        break;
                    }
                }
                game.MySelfInfo.奥秘列表.Remove(Hit);
            }
            else
            {
                game.YourInfo.SecretCount--;
            }
        }

        /// <summary>
        /// 运行奥秘
        /// </summary>
        /// <param name="SecretCardSN"></param>
        /// <param name="ActionCode"></param>
        /// <param name="HitMySelf"></param>
        /// <returns></returns>
        public static List<String> RunSecretHit(String SecretCardSN, String ActionCode, Boolean HitMySelf, Engine.Client.GameManager game)
        {
            List<String> ActionLst = new List<string>();
            SecretCard card = (SecretCard)CardUtility.GetCardInfoBySN(SecretCardSN);
            var actiontype = Engine.Server.ActionCode.GetActionType(ActionCode);
            var actionField = ActionCode.Split(Engine.Utility.CardUtility.strSplitMark.ToCharArray());
            ActionLst.Add(Engine.Server.ActionCode.strHitSecret + CardUtility.strSplitMark + (HitMySelf ? CardUtility.strMe : CardUtility.strYou) +
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
                        //PointEffect.RunPointEffect(game.YourInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1],card.AdditionInfo);
                        ActionLst.Add(Engine.Server.ActionCode.strPoint + Engine.Utility.CardUtility.strSplitMark + CardUtility.strYou + Engine.Utility.CardUtility.strSplitMark +
                                    actionField[3] + Engine.Utility.CardUtility.strSplitMark + card.AdditionInfo);
                    }
                    else
                    {
                        //在自己的回合运行别人的奥秘
                        if (actiontype == Server.ActionCode.ActionType.Summon)
                        {
                            //SUMMON#YOU#M000001#POS
                            //PointEffect.RunPointEffect(game.MyInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1], card.AdditionInfo);
                            ActionLst.Add(Engine.Server.ActionCode.strPoint + Engine.Utility.CardUtility.strSplitMark + CardUtility.strMe + Engine.Utility.CardUtility.strSplitMark +
                                    actionField[3] + Engine.Utility.CardUtility.strSplitMark + card.AdditionInfo);
                        }
                        else
                        {
                            //MINION#M000001#1
                            //PointEffect.RunPointEffect(game.MyInfo.BattleField.BattleMinions[int.Parse(actionField[2]) - 1], card.AdditionInfo);
                            ActionLst.Add(Engine.Server.ActionCode.strPoint + Engine.Utility.CardUtility.strSplitMark + CardUtility.strMe + Engine.Utility.CardUtility.strSplitMark +
                                    actionField[2] + Engine.Utility.CardUtility.strSplitMark + card.AdditionInfo);
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
