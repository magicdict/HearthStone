using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 运行效果
    /// </summary>
    public class CardEffect
    {
        /// <summary>
        /// 指定卡牌编号
        /// </summary>
        public String 指定卡牌编号 = String.Empty;
        /// <summary>
        /// 法术执行
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<string> RunEffect(GameManager game, CardUtility.TargetSelectDirectEnum Direct)
        {
            List<string> Result = new List<string>();
            switch (Direct)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    //#CARD#ME#M000001
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        var drawCards = Engine.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), true, 1);
                        if (drawCards.Count == 1)
                        {
                            game.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.HostInfo.HandCardCount++;
                            game.HostInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.HostSelfInfo.handCards.Add((Engine.Utility.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.HostInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        if (game.GuestInfo.RemainCardDeckCount > 0)
                        {
                            game.GuestInfo.HandCardCount++;
                            game.GuestInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                        }
                    }
                    else
                    {
                        game.GuestInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + 指定卡牌编号);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        var drawCards = Engine.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), true, 1);
                        if (drawCards.Count == 1)
                        {
                            game.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.HostInfo.HandCardCount++;
                            game.HostInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.HostSelfInfo.handCards.Add((Engine.Utility.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.HostInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        if (game.GuestInfo.RemainCardDeckCount > 0)
                        {
                            game.GuestInfo.HandCardCount++;
                            game.GuestInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                        }
                    }
                    else
                    {
                        game.GuestInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + 指定卡牌编号);
                    }
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 对方复原操作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        public static void ReRunEffect(GameManager game, String[] actField)
        {
            if (actField[1] == CardUtility.strYou)
            {
                if (actField.Length == 3)
                {
                    //如果有第三参数，则获得指定手牌
                    game.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(actField[2]));
                    game.HostInfo.HandCardCount++;
                }
                else
                {
                    var drawCards = Engine.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat),true, 1);
                    game.HostSelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                    game.HostInfo.HandCardCount++;
                    game.HostInfo.RemainCardDeckCount--;
                }
            }
            else
            {
                game.GuestInfo.HandCardCount++;
                game.GuestInfo.RemainCardDeckCount--;
            }
        }
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        public void GetField(List<string> InfoArray)
        {
            指定卡牌编号 = InfoArray[0];
        }
    }
}
