using Engine.Card;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Client
{
    /// <summary>
    /// 游戏状态[不能包含任何访问服务器的方法]
    /// </summary>
    public class GameStatus
    {
        /// <summary>
        /// GameId[抽牌魔法的时候，BS/CS都需要知道GameID]
        /// </summary>
        public int GameId;
        /// <summary>
        /// 客户信息
        /// </summary>
        public ClientInfo client = new ClientInfo();
        /// <summary>
        /// 客户端状态信息
        /// </summary>
        public struct ClientInfo
        {
            /// <summary>
            /// 游戏玩家名称
            /// </summary>
            public String PlayerNickName;
            /// <summary>
            /// 是否主机
            /// </summary>
            public Boolean IsHost;
            /// <summary>
            /// 是否为先手
            /// </summary>
            public Boolean IsFirst;
            /// <summary>
            /// 本方回合
            /// </summary>
            public Boolean IsMyTurn;
            /// <summary>
            /// 主机私有情报
            /// </summary>
            public PrivateInfo HostSelfInfo;
            /// <summary>
            /// 主机情报
            /// </summary>
            public PublicInfo HostInfo;
            /// <summary>
            /// 从属情报
            /// </summary>
            public PublicInfo GuestInfo;
            /// <summary>
            /// 从属私有情报
            /// </summary>
            public PrivateInfo GuestSelfInfo;
            /// <summary>
            /// 本方情报
            /// </summary>
            public PublicInfo MyInfo;
            /// <summary>
            /// 本方情报[CS]
            /// </summary>
            public PrivateInfo MySelfInfo;
            /// <summary>
            /// 对方情报
            /// </summary>
            public PublicInfo YourInfo;
            /// <summary>
            /// 对方情报[单机]
            /// </summary>
            public PrivateInfo YourSelfInfo;
            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                HostSelfInfo = new PrivateInfo();
                HostInfo = new PublicInfo();
                GuestInfo = new PublicInfo();
                GuestSelfInfo = new PrivateInfo();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetGameInfo()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine("本方手牌数：" + client.MyInfo.HandCardCount);
            info.AppendLine("对方手牌数：" + client.YourInfo.HandCardCount);
            info.AppendLine("本方剩余牌数：" + client.MyInfo.RemainCardDeckCount);
            info.AppendLine("对方剩余牌数：" + client.YourInfo.RemainCardDeckCount);
            return info.ToString();
        }
    }
}
