using Card.Client;
using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 运行效果
    /// </summary>
    public class CardEffect : AtomicEffectDefine
    {
        /// <summary>
        /// 法术方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum 法术方向 = CardUtility.TargetSelectDirectEnum.双方;
        /// <summary>
        /// 指定卡牌编号
        /// </summary>
        public String 指定卡牌编号 = String.Empty;
        /// <summary>
        /// 抽牌次数
        /// </summary>
        public String 抽牌次数表达式 = String.Empty;
        /// <summary>
        /// 法术执行
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<string> RunEffect(GameManager game)
        {
            List<string> Result = new List<string>();
            switch (法术方向)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    //#CARD#ME#M000001
                    if (String.IsNullOrEmpty(指定卡牌编号))
                    {
                        var drawCards = Card.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                        if (drawCards.Count == 1)
                        {
                            game.MySelfInfo.handCards.Add(Card.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.MyInfo.HandCardCount++;
                            game.MyInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.MySelfInfo.handCards.Add((Card.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.MyInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (String.IsNullOrEmpty(指定卡牌编号))
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
                    if (String.IsNullOrEmpty(指定卡牌编号))
                    {
                        var drawCards = Card.Client.ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                        if (drawCards.Count == 1)
                        {
                            game.MySelfInfo.handCards.Add(Card.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.MyInfo.HandCardCount++;
                            game.MyInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.MySelfInfo.handCards.Add((Card.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.MyInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    if (String.IsNullOrEmpty(指定卡牌编号))
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
        /// 初始化值
        /// </summary>
        public new void GetField()
        {
            法术方向 = CardUtility.GetEnum<CardUtility.TargetSelectDirectEnum>(InfoArray[1], CardUtility.TargetSelectDirectEnum.双方);
            抽牌次数表达式 = InfoArray[2];
            指定卡牌编号 = InfoArray[3];
        }
    }
}
