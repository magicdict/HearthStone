using Engine.Action;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
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
        public string 指定卡牌编号 = string.Empty;
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
                    DrawMyCard(game, Result);
                    break;
                case CardUtility.目标选择方向枚举.对方:
                    DrawYourCard(game, Result);
                    break;
                case CardUtility.目标选择方向枚举.双方:
                    DrawMyCard(game, Result);
                    DrawYourCard(game, Result);
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 抽牌【本方】
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Result"></param>
        private void DrawYourCard(ActionStatus game, List<string> Result)
        {
            if (string.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
            {
                if (game.AllRole.YourPublicInfo.RemainCardDeckCount > 0)
                {
                    if (SystemManager.游戏类型 == SystemManager.GameType.HTML版)
                    {
                        var drawCards = GameServer.DrawCard(game.GameId, !game.IsHost, 1);
                        game.AllRole.YourPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(drawCards[0]));
                    }
                    game.AllRole.YourPublicInfo.HandCardCount++;
                    game.AllRole.YourPublicInfo.RemainCardDeckCount--;
                    Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou);
                }
            }
            else
            {
                if (SystemManager.游戏类型 == SystemManager.GameType.HTML版)
                {
                    game.AllRole.YourPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(指定卡牌编号));
                }
                game.AllRole.YourPublicInfo.HandCardCount++;
                Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strYou + CardUtility.strSplitMark + 指定卡牌编号);
            }
        }
        /// <summary>
        /// 抽牌【对方】
        /// </summary>
        /// <param name="game"></param>
        /// <param name="Result"></param>
        private void DrawMyCard(ActionStatus game, List<string> Result)
        {
            if (string.IsNullOrEmpty(指定卡牌编号) || 指定卡牌编号 == CardUtility.strIgnore)
            {
                List<string> drawCards;
                if (SystemManager.游戏类型 == SystemManager.GameType.HTML版)
                {
                    drawCards = GameServer.DrawCard(game.GameId, game.IsHost, 1);
                }
                else
                {
                    drawCards = ClientRequest.DrawCard(game.GameId.ToString(GameServer.GameIdFormat), game.IsHost, 1);
                }
                if (drawCards.Count == 1)
                {
                    game.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(drawCards[0]));
                    game.AllRole.MyPublicInfo.HandCardCount++;
                    game.AllRole.MyPublicInfo.RemainCardDeckCount--;
                    Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
                }
            }
            else
            {
                game.AllRole.MyPrivateInfo.handCards.Add((CardUtility.GetCardInfoBySN(指定卡牌编号)));
                game.AllRole.MyPublicInfo.HandCardCount++;
                Result.Add(ActionCode.strCard + CardUtility.strSplitMark + CardUtility.strMe);
            }
        }

        /// <summary>
        /// 对方复原操作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="actField"></param>
        public static void ReRunEffect(int GameId, ActionStatus game, string[] actField)
        {
            if (actField[1] == CardUtility.strYou)
            {
                if (actField.Length == 3)
                {
                    //如果有第三参数，则获得指定手牌
                    game.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(actField[2]));
                    game.AllRole.MyPublicInfo.HandCardCount++;
                }
                else
                {
                    var drawCards = ClientRequest.DrawCard(GameId.ToString(GameServer.GameIdFormat), game.IsHost, 1);
                    game.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(drawCards[0]));
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
