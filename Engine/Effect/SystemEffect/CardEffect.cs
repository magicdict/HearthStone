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
        public List<string> RunEffect(GameManager game,CardUtility.TargetSelectDirectEnum Direct)
        {
            List<string> Result = new List<string>();
            switch (Direct)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    //#CARD#ME#M000001
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        var drawCards = Engine.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                        if (drawCards.Count == 1)
                        {
                            game.MySelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.MyInfo.HandCardCount++;
                            game.MyInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.MySelfInfo.handCards.Add((Engine.Utility.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.MyInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        if (game.YourInfo.RemainCardDeckCount > 0)
                        {
                            game.YourInfo.HandCardCount++;
                            game.YourInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                        }
                    }
                    else
                    {
                        game.YourInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + 指定卡牌编号);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        var drawCards = Engine.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                        if (drawCards.Count == 1)
                        {
                            game.MySelfInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.MyInfo.HandCardCount++;
                            game.MyInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.MySelfInfo.handCards.Add((Engine.Utility.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.MyInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        if (game.YourInfo.RemainCardDeckCount > 0)
                        {
                            game.YourInfo.HandCardCount++;
                            game.YourInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                        }
                    }
                    else
                    {
                        game.YourInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + 指定卡牌编号);
                    }
                    break;
                default:
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InfoArray"></param>
        public void GetField(List<string> InfoArray)
        {
            指定卡牌编号 = InfoArray[0];
        }
    }
}
