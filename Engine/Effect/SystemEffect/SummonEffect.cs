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
        public List<string> RunEffect(Client.GameManager game, Utility.CardUtility.TargetSelectDirectEnum Direct)
        {
            List<String> Result = new List<string>();
            var MinionLst = 指定卡牌编号数组.Split(Engine.Utility.CardUtility.strSplitMark.ToCharArray());
            Random random = new Random(DateTime.Now.Millisecond);
            var CardSN = MinionLst[random.Next(0, MinionLst.Length)];
            var Minion = (Engine.Card.MinionCard)Engine.Utility.CardUtility.GetCardInfoBySN(CardSN);
            switch (Direct)
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
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
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
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
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
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
                        });
                    }
                    if (game.YourInfo.BattleField.MinionCount < Engine.Client.BattleFieldInfo.MaxMinionCount)
                    {
                        game.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strYou +
                            Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.YourInfo.BattleField.MinionCount);
                        game.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
                        });
                    }
                    break;
                default:
                    break;
            }
            return Result;
        }
        /// <summary>
        /// 获得效果信息
        /// </summary>
        /// <param name="InfoArray"></param>
        public void GetField(List<string> InfoArray)
        {
            指定卡牌编号数组 = InfoArray[0];
        }
    }
}
