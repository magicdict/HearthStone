using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Effect
{
    /// <summary>
    /// 召唤效果
    /// </summary>
    public class SummonEffect 
    {
        /// <summary>
        /// 法术方向
        /// </summary>
        public CardUtility.TargetSelectDirectEnum 法术方向 = CardUtility.TargetSelectDirectEnum.双方;
        /// <summary>
        /// 
        /// </summary>
        public String 指定卡牌编号数组 = String.Empty;
        /// <summary>
        /// 运行效果
        /// </summary>
        /// <param name="singleEffect"></param>
        /// <param name="game"></param>
        /// <param name="Seed"></param>
        /// <returns></returns>
        public List<string> RunEffect(Client.GameManager game)
        {
            List<String> Result = new List<string>();
            var MinionLst = 指定卡牌编号数组.Split(Engine.Utility.CardUtility.strSplitMark.ToCharArray());
            Random random = new Random(DateTime.Now.Millisecond);
            var CardSN = MinionLst[random.Next(0, MinionLst.Length)];
            var Minion = (Engine.Card.MinionCard)Engine.Utility.CardUtility.GetCardInfoBySN(CardSN);
            switch (法术方向)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    if (game.MyInfo.BattleField.MinionCount < Engine.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.MyInfo.BattleField.AppendToBattle(Minion);
                        //SUMMON#YOU#M000001#POS
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strMe +
                                   Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.MyInfo.BattleField.MinionCount);
                        game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (game.YourInfo.BattleField.MinionCount < Engine.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strYou +
                            Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.YourInfo.BattleField.MinionCount);
                        game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.对方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    if (game.MyInfo.BattleField.MinionCount < Engine.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.MyInfo.BattleField.AppendToBattle(Minion);
                        //SUMMON#YOU#M000001#POS
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strMe +
                                   Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.MyInfo.BattleField.MinionCount);
                        game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            事件类型 = CardUtility.事件类型列表.召唤,
                            触发方向 = CardUtility.TargetSelectDirectEnum.本方,
                            附加信息 = Minion.种族.ToString(),
                            触发位置 = game.MyInfo.BattleField.MinionCount
                        });
                    }
                    if (game.YourInfo.BattleField.MinionCount < Engine.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strYou +
                            Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.YourInfo.BattleField.MinionCount);
                        game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
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
