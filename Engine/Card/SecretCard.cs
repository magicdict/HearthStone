using Engine.Action;
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
        public string AdditionInfo = string.Empty;
        /// <summary>
        /// 是否已经触发
        /// </summary>
        public bool IsHitted = false;
        /// <summary>
        /// 奥秘是否命中
        /// </summary>
        /// <param name="SecretCardSN"></param>
        /// <param name="ActionCode"></param>
        /// <param name="HitMySelf">是否检查自己是否触发自己的奥秘</param>
        /// <returns></returns>
        public static bool IsSecretHit(string SecretCardSN, string ActionCode, bool HitMySelf)
        {
            //HitMySelf 
            //这里需要关注方向性问题
            SecretCard card = (SecretCard)CardUtility.GetCardInfoBySN(SecretCardSN);
            var actiontype = Server.ActionCode.GetActionType(ActionCode);
            var actionField = ActionCode.Split(CardUtility.strSplitMark.ToCharArray());
            bool IsHit = false;
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
        public static bool IsSecretAction(string actionDetail)
        {
            if (actionDetail.StartsWith(ActionCode.strSecret + CardUtility.strSplitMark))
            {
                return true;
            }
            return false;
            //    //使用奥秘
            //    String SecretCardSN = actionDetail.Substring(ActionCode.strSecret.Length + Engine.Utility.CardUtility.strSplitMark.Length);
            //    if (上下半局)
            //    {
            //        //HostStatus.SelfInfo.奥秘列表.Add(CardUtility.GetCardInfoBySN(SecretCardSN));
            //    }
            //    else
            //    {
            //        //gameStatus.GuestSecret.Add(SecretCardSN);
            //    }
            //    //奥秘的时候，不放松奥秘内容
            //    //注意和ActionCode.GetActionType()保持一致
            //    ActionInfo.Add(ActionCode.strSecret);
            //}
            //else
            //{
            //    //奥秘判断 注意：这个动作需要改变FirstSecret和SecondSecret
            //    if (actionDetail.StartsWith(ActionCode.strHitSecret))
            //    {
            //        var secretInfo = actionDetail.Split(CardUtility.strSplitMark.ToCharArray());
            //        if (上下半局)
            //        {
            //            //先手
            //            if (secretInfo[1] == CardUtility.strMe)
            //            {
            //                //HostSecret.Remove(secretInfo[2]);
            //            }
            //            else
            //            {
            //                //GuestSecret.Remove(secretInfo[2]);
            //            }
            //        }
            //        else
            //        {
            //            //后手
            //            if (secretInfo[1] == CardUtility.strMe)
            //            {
            //                //gameStatus.GuestSecret.Remove(secretInfo[2]);
            //            }
            //            else
            //            {
            //                //gameStatus.HostSecret.Remove(secretInfo[2]);
            //            }
            //        }
            //    }
            //    //动作写入
            //}
        }
        /// <summary>
        /// 奥秘计算
        /// </summary>
        /// <param name="actionlst"></param>
        /// <returns></returns>
        public static List<string> 奥秘计算(List<string> actionlst, ActionStatus game, int GameId)
        {
            List<string> Result = new List<string>();
            //奥秘计算 START
            //本方（Fight也需要）
            if (game.AllRole.MyPrivateInfo.奥秘列表.Count != 0)
            {
                //本方的行动触发本方奥秘的检查
                for (int i = 0; i < actionlst.Count; i++)
                {
                    foreach (var secret in game.AllRole.MyPrivateInfo.奥秘列表)
                    {
                        if ((!secret.IsHitted) && IsSecretHit(secret.序列号, actionlst[i], true))
                        {
                            //奥秘执行
                            Result.AddRange(RunSecretHit(secret.序列号, actionlst[i], true, game));
                            secret.IsHitted = true;
                        }
                    }
                }
                //移除已经触发的奥秘
                game.AllRole.MyPrivateInfo.清除命中奥秘();
            }
            //对方（Fight也需要）
            if (game.AllRole.YourPublicInfo.Hero.SecretCount != 0)
            {
                var HitCard = ClientRequest.IsSecretHit(GameId.ToString(GameServer.GameIdFormat), true, actionlst);
                if (!string.IsNullOrEmpty(HitCard))
                {
                    var HitCardList = HitCard.Split(CardUtility.strSplitArrayMark.ToCharArray());
                    foreach (var hitCard in HitCardList)
                    {
                        Result.AddRange(RunSecretHit(hitCard.Split(CardUtility.strSplitDiffMark.ToCharArray())[0],
                                                                     hitCard.Split(CardUtility.strSplitDiffMark.ToCharArray())[1], false, game));
                        game.AllRole.YourPublicInfo.Hero.SecretCount--;
                    }
                }
            }
            //奥秘计算 END
            Result.AddRange(ActionStatus.Settle(game));
            return Result;
        }
        /// <summary>
        /// 是否HIT对方奥秘
        /// </summary>
        /// <param name="IsFirst">是否为先手</param>
        /// <returns></returns>
        public string SecretHitCheck(string Action, bool IsFirst, ActionStatus gameStatus)
        {
            //奥秘判断 注意：这个动作并不改变FirstSecret和SecondSecret
            //1.例如，发生战斗的时候，如果两个随从都死了，
            //同时两边都有随从死亡的奥秘，则整个动作序列可能触发两边的奥秘
            //<本方奥秘在客户端判断>注意方向
            //2.服务器端只做判断，并且返回命中奥秘的列表，不做任何其他操作！
            List<string> HITCardList = new List<string>();
            foreach (var actionDetail in Action.Split(CardUtility.strSplitArrayMark.ToCharArray()))
            {
                //检查Second
                if (IsFirst && gameStatus.AllRole.YourPrivateInfo.奥秘列表.Count != 0)
                {
                    for (int i = 0; i < gameStatus.AllRole.YourPrivateInfo.奥秘列表.Count; i++)
                    {
                        if (IsSecretHit(gameStatus.AllRole.YourPrivateInfo.奥秘列表[i].序列号, actionDetail, false))
                        {
                            HITCardList.Add(gameStatus.AllRole.YourPrivateInfo.奥秘列表[i] + CardUtility.strSplitDiffMark + actionDetail);
                        }
                    }
                }
                //检查First
                if ((!IsFirst) && gameStatus.AllRole.MyPrivateInfo.奥秘列表.Count != 0)
                {
                    for (int i = 0; i < gameStatus.AllRole.MyPrivateInfo.奥秘列表.Count; i++)
                    {
                        if (IsSecretHit(gameStatus.AllRole.MyPrivateInfo.奥秘列表[i].序列号, actionDetail, false))
                        {
                            HITCardList.Add(gameStatus.AllRole.MyPrivateInfo.奥秘列表[i] + CardUtility.strSplitDiffMark + actionDetail);
                        }
                    }
                }
            }
            return HITCardList.ToListString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        public static void ReRunSecret(ActionStatus game, string[] actField)
        {
            if (actField[1] == CardUtility.strYou)
            {
                SecretCard Hit = new SecretCard();
                foreach (var secret in game.AllRole.MyPrivateInfo.奥秘列表)
                {
                    if (secret.序列号 == actField[2])
                    {
                        Hit = secret;
                        break;
                    }
                }
                game.AllRole.MyPrivateInfo.奥秘列表.Remove(Hit);
            }
            else
            {
                game.AllRole.YourPublicInfo.Hero.SecretCount--;
            }
        }

        /// <summary>
        /// 运行奥秘
        /// </summary>
        /// <param name="SecretCardSN"></param>
        /// <param name="ActionCode"></param>
        /// <param name="HitMySelf"></param>
        /// <returns></returns>
        public static List<string> RunSecretHit(string SecretCardSN, string ActionCode, bool HitMySelf, ActionStatus game)
        {
            List<string> ActionLst = new List<string>();
            SecretCard card = (SecretCard)CardUtility.GetCardInfoBySN(SecretCardSN);
            var actiontype = Server.ActionCode.GetActionType(ActionCode);
            var actionField = ActionCode.Split(CardUtility.strSplitMark.ToCharArray());
            ActionLst.Add(Server.ActionCode.strHitSecret + CardUtility.strSplitMark + (HitMySelf ? CardUtility.strMe : CardUtility.strYou) +
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
                        //PointEffect.RunPointEffect(game.AllRole.YourBattleInfo.BattleMinions[int.Parse(actionField[3]) - 1],card.AdditionInfo);
                        ActionLst.Add(Server.ActionCode.strPoint + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark +
                                    actionField[3] + CardUtility.strSplitMark + card.AdditionInfo);
                    }
                    else
                    {
                        //在自己的回合运行别人的奥秘
                        if (actiontype == Server.ActionCode.ActionType.Summon)
                        {
                            //SUMMON#YOU#M000001#POS
                            //PointEffect.RunPointEffect(game.MyInfo.BattleField.BattleMinions[int.Parse(actionField[3]) - 1], card.AdditionInfo);
                            ActionLst.Add(Server.ActionCode.strPoint + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark +
                                    actionField[3] + CardUtility.strSplitMark + card.AdditionInfo);
                        }
                        else
                        {
                            //MINION#M000001#1
                            //PointEffect.RunPointEffect(game.MyInfo.BattleField.BattleMinions[int.Parse(actionField[2]) - 1], card.AdditionInfo);
                            ActionLst.Add(Server.ActionCode.strPoint + CardUtility.strSplitMark + CardUtility.strMe + CardUtility.strSplitMark +
                                    actionField[2] + CardUtility.strSplitMark + card.AdditionInfo);
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
