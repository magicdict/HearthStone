using Card.Client;
using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 
    /// </summary>
    public class CardEffect : EffectDefine
    {
        public static List<string> RunEffect(EffectDefine singleEffect, GameManager game)
        {
            List<string> Result = new List<string>();
            switch (singleEffect.EffectTargetSelectDirect)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    //#CARD#ME#M000001
                    var drawCards = Card.Server.ClientUtlity.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                    if (drawCards.Count == 1)
                    {
                        game.MySelf.handCards.Add(Card.CardUtility.GetCardInfoBySN(drawCards[0]));
                        game.MySelf.RoleInfo.HandCardCount++;
                        game.MySelf.RoleInfo.RemainCardDeckCount--;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (game.YourInfo.RemainCardDeckCount > 0)
                    {
                        game.YourInfo.HandCardCount++;
                        game.YourInfo.RemainCardDeckCount--;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    var drawCardsT = Card.Server.ClientUtlity.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsFirst, 1);
                    if (drawCardsT.Count == 1)
                    {
                        game.MySelf.handCards.Add(Card.CardUtility.GetCardInfoBySN(drawCardsT[0]));
                        game.MySelf.RoleInfo.HandCardCount++;
                        game.MySelf.RoleInfo.RemainCardDeckCount--;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    if (game.YourInfo.RemainCardDeckCount > 0)
                    {
                        game.YourInfo.HandCardCount++;
                        game.YourInfo.RemainCardDeckCount--;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                    }
                    break;
                default:
                    break;
            }
            return Result;
        }
    }
}
