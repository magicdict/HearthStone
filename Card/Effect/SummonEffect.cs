using System;
using System.Collections.Generic;

namespace Card.Effect
{
    /// <summary>
    /// 召唤效果
    /// </summary>
    public class SummonEffect:AtomicEffectDefine
    {
        /// <summary>
        /// 法术方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum 法术方向 = CardUtility.TargetSelectDirectEnum.双方;
        /// <summary>
        /// 运行效果
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="Seed"></param>
        /// <returns></returns>
        public new List<string> RunEffect(Client.GameManager game)
        {
            List<String> Result = new List<string>();
            var MinionLst = AdditionInfo.Split(Card.CardUtility.strSplitMark.ToCharArray());
            Random random = new Random(DateTime.Now.Millisecond);
            var CardSN = MinionLst[random.Next(0, MinionLst.Length)];
            var Minion = (Card.MinionCard)Card.CardUtility.GetCardInfoBySN(CardSN);
            switch (法术方向)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    if (game.MyInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.MyInfo.BattleField.AppendToBattle(Minion);
                        //SUMMON#YOU#M000001#POS
                        Result.Add(Card.Server.ActionCode.strSummon + Card.CardUtility.strSplitMark + Card.CardUtility.strMe +
                                   Card.CardUtility.strSplitMark + CardSN + Card.CardUtility.strSplitMark + game.MyInfo.BattleField.MinionCount);
                        game.事件池.Add(new Card.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (game.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Card.Server.ActionCode.strSummon + Card.CardUtility.strSplitMark + Card.CardUtility.strYou +
                            Card.CardUtility.strSplitMark + CardSN + Card.CardUtility.strSplitMark + game.YourInfo.BattleField.MinionCount);
                        game.事件池.Add(new Card.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    if (game.MyInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.MyInfo.BattleField.AppendToBattle(Minion);
                        //SUMMON#YOU#M000001#POS
                        Result.Add(Card.Server.ActionCode.strSummon + Card.CardUtility.strSplitMark + Card.CardUtility.strMe +
                                   Card.CardUtility.strSplitMark + CardSN + Card.CardUtility.strSplitMark + game.MyInfo.BattleField.MinionCount);
                        game.事件池.Add(new Card.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    if (game.YourInfo.BattleField.MinionCount < Card.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Card.Server.ActionCode.strSummon + Card.CardUtility.strSplitMark + Card.CardUtility.strYou +
                            Card.CardUtility.strSplitMark + CardSN + Card.CardUtility.strSplitMark + game.YourInfo.BattleField.MinionCount);
                        game.事件池.Add(new Card.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    break;
                default:
                    break;
            }
            return Result;
        }
    }
}
