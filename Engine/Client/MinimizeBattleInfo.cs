using System;
using Engine.Utility;
using Engine.Card;

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
        /// <summary>
        /// 公开信息
        /// </summary>
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
            /// 
            /// </summary>
            /// <param name="pubInfo"></param>
            /// <param name="priInfo"></param>
            public void Init(PublicInfo pubInfo)
            {
                护盾值 = pubInfo.ShieldPoint;
                生命力 = pubInfo.LifePoint;
            }
        }
        public struct HandCardInfo
        {
            /// <summary>
            /// 序列号
            /// </summary>
            public String 序列号;
            /// <summary>
            /// 名称
            /// </summary>
            public String 名称;
            /// <summary>
            /// 成本
            /// </summary>
            public int 使用成本;
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="card"></param>
            public void Init(CardBasicInfo card)
            {
                序列号 = card.序列号;
                名称 = card.名称;
                使用成本 = card.使用成本;
            }
        }
        public Minion[] HostBattle;
        public Minion[] GuestBattle;
        public HandCardInfo[] HandCard;
        public PlayerInfo MyInfo = new PlayerInfo();
        public void Init(ClientPlayerInfo status, Boolean IsHost)
        {
            if (IsHost)
            {
                MyInfo.Init(status.HostInfo);
                HandCard = new HandCardInfo[status.HostSelfInfo.handCards.Count];
                for (int i = 0; i < status.HostSelfInfo.handCards.Count; i++)
                {
                    HandCardInfo t = new HandCardInfo();
                    t.Init(status.HostSelfInfo.handCards[i]);
                    HandCard[i] = t;
                }
            }
            else
            {
                HandCard = new HandCardInfo[status.GuestSelfInfo.handCards.Count];
                for (int i = 0; i < status.GuestSelfInfo.handCards.Count; i++)
                {
                    HandCardInfo t = new HandCardInfo();
                    t.Init(status.GuestSelfInfo.handCards[i]);
                    HandCard[i] = t;
                }
                MyInfo.Init(status.GuestInfo);
            }
            HostBattle = new Minion[status.HostInfo.BattleField.MinionCount];
            for (int i = 0; i < status.HostInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion();
                t.Init(status.HostInfo.BattleField.BattleMinions[i]);
                HostBattle[i] = t;
            }
            GuestBattle = new Minion[status.GuestInfo.BattleField.MinionCount];
            for (int i = 0; i < status.GuestInfo.BattleField.MinionCount; i++)
            {
                Minion t = new Minion();
                t.Init(status.GuestInfo.BattleField.BattleMinions[i]);
                GuestBattle[i] = t;
            }
        }
    }
}
