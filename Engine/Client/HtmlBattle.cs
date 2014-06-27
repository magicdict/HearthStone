using System;
using Engine.Utility;

namespace Engine.Client
{
    /// <summary>
    /// 随从最小化数据
    /// </summary>
    public class Minion
    {
        /// <summary>
        /// 攻击力
        /// </summary>
        public int 攻击力;
        /// <summary>
        /// 生命力
        /// </summary>
        public int 生命力;
        /// <summary>
        /// 状态列表
        /// </summary>
        public String 状态列表;
        /// <summary>
        /// InitByMinion
        /// </summary>
        /// <param name="minion"></param>
        public Minion(Card.MinionCard minion)
        {
            攻击力 = minion.实际攻击值;
            生命力 = minion.生命值;
            状态列表 = minion.状态;
        }
    }
    /// <summary>
    /// 战场最小化数据
    /// </summary>
    public class Battle
    {
        Minion[] HostBattle = new Minion[SystemManager.MaxMinionCount];
        Minion[] GuestBattle = new Minion[SystemManager.MaxMinionCount];
        public Battle(GameStatus status)
        {
            for (int i = 0; i < status.client.HostInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion(status.client.HostInfo.BattleField.BattleMinions[i]);
                HostBattle[i] = t;
            }
            for (int i = 0; i < status.client.GuestInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion(status.client.GuestInfo.BattleField.BattleMinions[i]);
                GuestBattle[i] = t;
            }
        }
    }
}
