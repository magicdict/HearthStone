using System;
using Engine.Utility;

namespace Engine.Client
{
    public class MinimizeBattleInfo
    {
        /// <summary>
        /// 随从最小化数据
        /// </summary>
        public struct Minion
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
            public void Init(Card.MinionCard minion)
            {
                攻击力 = minion.实际攻击值;
                生命力 = minion.生命值;
                状态列表 = minion.状态;
            }
        }
        public struct PlayerInfo
        {
            /// <summary>
            /// 护盾值
            /// </summary>
            public int 护盾值;
            /// <summary>
            /// 生命力
            /// </summary>
            public int 生命力;
            /// <summary>
            /// 手牌
            /// </summary>
            public string[] 手牌;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="pubInfo"></param>
            /// <param name="priInfo"></param>
            public void Init(PublicInfo pubInfo, PrivateInfo priInfo)
            {
                护盾值 = pubInfo.ShieldPoint;
                生命力 = pubInfo.LifePoint;
                手牌 = new string[priInfo.handCards.Count];
                for (int i = 0; i < priInfo.handCards.Count; i++)
                {
                    手牌[i] = priInfo.handCards[i].名称;
                }
            }
        }
        public Minion[] HostBattle;
        public Minion[] GuestBattle; 
        public PlayerInfo MyInfo = new PlayerInfo();
        public void Init(GameStatus status, Boolean IsHost)
        {
            if (IsHost)
            {
                MyInfo.Init(status.client.HostInfo, status.client.HostSelfInfo);
            }
            else
            {
                MyInfo.Init(status.client.GuestInfo, status.client.GuestSelfInfo);
            }
            HostBattle = new Minion[status.client.HostInfo.BattleField.MinionCount];
            for (int i = 0; i < status.client.HostInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion();
                t.Init(status.client.HostInfo.BattleField.BattleMinions[i]);
                HostBattle[i] = t;
            }
            GuestBattle = new Minion[status.client.GuestInfo.BattleField.MinionCount];
            for (int i = 0; i < status.client.GuestInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion();
                t.Init(status.client.GuestInfo.BattleField.BattleMinions[i]);
                GuestBattle[i] = t;
            }
        }
    }
}
