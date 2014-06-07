using Card.Client;
using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 运行效果
    /// </summary>
    public class CardEffect : EffectDefine
    {
        public static List<string> RunEffect(EffectDefine singleEffect, GameManager game)
        {
            List<string> Result = new List<string>();
            switch (singleEffect.SelectOpt.EffectTargetSelectDirect)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    //#CARD#ME#M000001
                    if (String.IsNullOrEmpty(singleEffect.AddtionInfo))
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
                        game.MySelfInfo.handCards.Add((Card.CardUtility.GetCardInfoBySN(singleEffect.AddtionInfo)));
                        game.MyInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (String.IsNullOrEmpty(singleEffect.AddtionInfo))
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
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + singleEffect.AddtionInfo);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    if (String.IsNullOrEmpty(singleEffect.AddtionInfo))
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
                        game.MySelfInfo.handCards.Add((Card.CardUtility.GetCardInfoBySN(singleEffect.AddtionInfo)));
                        game.MyInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    if (String.IsNullOrEmpty(singleEffect.AddtionInfo))
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
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + singleEffect.AddtionInfo);
                    }
                    break;
                default:
                    break;
            }
            return Result;
        }
    }
}
