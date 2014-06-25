using Engine.Client;
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
        public List<string> RunEffect(Client.GameStatus game, Utility.CardUtility.TargetSelectDirectEnum Direct)
        {
            List<String> Result = new List<string>();
            var MinionLst = 指定卡牌编号数组.Split(Engine.Utility.CardUtility.strSplitMark.ToCharArray());
            Random random = new Random(DateTime.Now.Millisecond);
            var CardSN = MinionLst[random.Next(0, MinionLst.Length)];
            var Minion = (Engine.Card.MinionCard)Engine.Utility.CardUtility.GetCardInfoBySN(CardSN);
            switch (Direct)
            {
                case CardUtility.TargetSelectDirectEnum.本方:
                    if (game.client.MyInfo.BattleField.MinionCount < SystemManager.MaxMinionCount)
                    {
                        game.client.MyInfo.BattleField.AppendToBattle(Minion);
                        //SUMMON#YOU#M000001#POS
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strMe +
                                   Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.client.MyInfo.BattleField.MinionCount);
                        GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
                        });
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.对方:
                    if (game.client.YourInfo.BattleField.MinionCount < SystemManager.MaxMinionCount)
                    {
                        game.client.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strYou +
                            Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.client.YourInfo.BattleField.MinionCount);
                        GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
                        });
                    }
                    break;
                case CardUtility.TargetSelectDirectEnum.双方:
                    if (game.client.MyInfo.BattleField.MinionCount < SystemManager.MaxMinionCount)
                    {
                        game.client.MyInfo.BattleField.AppendToBattle(Minion);
                        //SUMMON#YOU#M000001#POS
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strMe +
                                   Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.client.MyInfo.BattleField.MinionCount);
                        GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
                        });
                    }
                    if (game.client.YourInfo.BattleField.MinionCount < SystemManager.MaxMinionCount)
                    {
                        game.client.YourInfo.BattleField.AppendToBattle(Minion);
                        Result.Add(Engine.Server.ActionCode.strSummon + Engine.Utility.CardUtility.strSplitMark + Engine.Utility.CardUtility.strYou +
                            Engine.Utility.CardUtility.strSplitMark + CardSN + Engine.Utility.CardUtility.strSplitMark + game.client.YourInfo.BattleField.MinionCount);
                        GameManager.事件处理组件.事件池.Add(new Engine.Utility.CardUtility.全局事件()
                        {
                            触发事件类型 = CardUtility.事件类型列表.召唤,
                            触发位置 = Minion.战场位置
                        });
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
        public static void ReRunEffect(GameStatus game, String[] actField)
        {
            //不会出现溢出的问题，溢出在Effect里面处理过了
            //SUMMON#YOU#M000001
            //Me代表对方 YOU代表自己，必须反过来
            if (actField[1] == CardUtility.strYou)
            {
                game.client.MyInfo.BattleField.AppendToBattle(actField[2]);
            }
            else
            {
                game.client.YourInfo.BattleField.AppendToBattle(actField[2]);
            }
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
