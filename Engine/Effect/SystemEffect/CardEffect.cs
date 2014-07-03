using Engine.Action;
using Engine.Client;
using Engine.Control;
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
        public List<string> RunEffect(ActionStatus game, CardUtility.目标选择方向枚举 Direct)
        {
            List<string> Result = new List<string>();
            switch (Direct)
            {
                case CardUtility.目标选择方向枚举.本方:
                    //#CARD#ME#M000001
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        var drawCards = ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsHost, 1);
                        if (drawCards.Count == 1)
                        {
                            game.AllRole.MyPrivateInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.AllRole.MyPublicInfo.HandCardCount++;
                            game.AllRole.MyPublicInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.AllRole.MyPrivateInfo.handCards.Add((Engine.Utility.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.AllRole.MyPublicInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    break;
                case CardUtility.目标选择方向枚举.对方:
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        if (game.AllRole.YourPublicInfo.RemainCardDeckCount > 0)
                        {
                            game.AllRole.YourPublicInfo.HandCardCount++;
                            game.AllRole.YourPublicInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                        }
                    }
                    else
                    {
                        game.AllRole.YourPublicInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + 指定卡牌编号);
                    }
                    break;
                case CardUtility.目标选择方向枚举.双方:
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        var drawCards = ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsHost, 1);
                        if (drawCards.Count == 1)
                        {
                            game.AllRole.MyPrivateInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                            game.AllRole.MyPublicInfo.HandCardCount++;
                            game.AllRole.MyPublicInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                        }
                    }
                    else
                    {
                        game.AllRole.MyPrivateInfo.handCards.Add((Engine.Utility.CardUtility.GetCardInfoBySN(指定卡牌编号)));
                        game.AllRole.MyPublicInfo.HandCardCount++;
                        Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                    }
                    if (String.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
                    {
                        if (game.AllRole.YourPublicInfo.RemainCardDeckCount > 0)
                        {
                            game.AllRole.YourPublicInfo.HandCardCount++;
                            game.AllRole.YourPublicInfo.RemainCardDeckCount--;
                            Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                        }
                    }
                    else
                    {
                        game.AllRole.YourPublicInfo.HandCardCount++;
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
        public static void ReRunEffect(int GameId, ActionStatus game, String[] actField)
        {
            if (actField[1] == CardUtility.strYou)
            {
                if (actField.Length == 3)
                {
                    //如果有第三参数，则获得指定手牌
                    game.AllRole.MyPrivateInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(actField[2]));
                    game.AllRole.MyPublicInfo.HandCardCount++;
                }
                else
                {
                    var drawCards = ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), game.IsHost, 1);
                    game.AllRole.MyPrivateInfo.handCards.Add(Engine.Utility.CardUtility.GetCardInfoBySN(drawCards[0]));
                    game.AllRole.MyPublicInfo.HandCardCount++;
                    game.AllRole.MyPublicInfo.RemainCardDeckCount--;
                }
            }
            else
            {
                game.AllRole.YourPublicInfo.HandCardCount++;
                game.AllRole.YourPublicInfo.RemainCardDeckCount--;
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
