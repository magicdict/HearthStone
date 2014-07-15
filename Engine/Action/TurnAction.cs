using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Action
{
    public static class TurnAction
    {
        /// <summary>
        /// 新的回合
        /// </summary>
        public static void TurnStart(ActionStatus gameStatus)
        {
            gameStatus.AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(ClientRequest.DrawCard(gameStatus.GameId.ToString(GameServer.GameIdFormat), gameStatus.IsHost, 1)[0]));
            //过载的清算
            if (gameStatus.AllRole.MyPublicInfo.OverloadPoint != 0)
            {
                gameStatus.AllRole.MyPublicInfo.crystal.ReduceCurrentPoint(gameStatus.AllRole.MyPublicInfo.OverloadPoint);
                gameStatus.AllRole.MyPublicInfo.OverloadPoint = 0;
            }
            //连击的重置
            gameStatus.AllRole.MyPublicInfo.连击状态 = false;
            //魔法水晶的增加
            gameStatus.AllRole.MyPublicInfo.crystal.NewTurn();
            gameStatus.AllRole.MyPublicInfo.RemainAttactTimes = 1;
            gameStatus.AllRole.MyPublicInfo.IsUsedHeroAbility = false;
            gameStatus.AllRole.MyPublicInfo.BattleField.FreezeStatus();
            //重置攻击次数,必须放在状态变化之后！
            //原因是剩余攻击回数和状态有关！
            foreach (var minion in gameStatus.AllRole.MyPublicInfo.BattleField.BattleMinions)
            {
                if (minion != null) minion.重置剩余攻击次数();
            }
        }
        /// <summary>
        /// 对手回合结束的清场
        /// </summary>
        public static List<String> TurnEnd(ActionStatus gameStatus)
        {
            List<String> ActionLst = new List<string>();
            //对手回合加成属性的去除
            int ExistMinionCount = gameStatus.AllRole.MyPublicInfo.BattleField.MinionCount;
            for (int i = 0; i < ExistMinionCount; i++)
            {
                if (gameStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i] != null)
                {
                    gameStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i].本回合生命力加成 = 0;
                    gameStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i].本回合攻击力加成 = 0;
                    if (gameStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i].特殊效果 == MinionCard.特殊效果枚举.回合结束死亡)
                    {
                        gameStatus.AllRole.MyPublicInfo.BattleField.BattleMinions[i] = null;
                    }
                }
            }
            gameStatus.AllRole.MyPublicInfo.BattleField.ClearDead(gameStatus.battleEvenetHandler, false);
            ActionLst.AddRange(ActionStatus.Settle(gameStatus));
            ActionLst.AddRange(gameStatus.battleEvenetHandler.事件处理(gameStatus));
            return ActionLst;
        }
    }
}
