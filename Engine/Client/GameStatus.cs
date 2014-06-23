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
        /// 游戏编号
        /// </summary>
        public int GameId;
        /// <summary>
        /// 游戏类型
        /// </summary>
        public SystemManager.GameType 游戏类型 = SystemManager.GameType.客户端服务器版;
        /// <summary>
        /// 客户信息
        /// </summary>
        public ClientInfo client = new ClientInfo();
        /// <summary>
        /// 服务器信息
        /// </summary>
        public ServerInfo server = new ServerInfo();
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
        /// 服务器端状态信息
        /// </summary>
        public struct ServerInfo
        {
            /// <summary>
            /// 主机作为先手
            /// </summary>
            public Boolean HostAsFirst;
            /// <summary>
            /// 是否先手
            /// </summary>
            /// <param name="IsHost"></param>
            /// <returns></returns>
            public Boolean IsFirst(Boolean IsHost)
            {
                if (IsHost && HostAsFirst) return true;
                if (!IsHost && !HostAsFirst) return true;
                return false;
            }
            /// <summary>
            /// 主机玩家名称
            /// </summary>
            public String HostNickName;
            /// <summary>
            /// 非主机玩家名称
            /// </summary>
            public String GuestNickName;
            /// <summary>
            /// 先手牌堆
            /// </summary>
            public CardDeck HostCardDeck;
            /// <summary>
            /// 先手奥秘
            /// </summary>
            public List<String> HostSecret;
            /// <summary>
            /// 后手牌堆
            /// </summary>
            public CardDeck GuestCardDeck;
            /// <summary>
            /// 后手奥秘
            /// </summary>
            public List<String> GuestSecret;
            /// <summary>
            /// 初始化
            /// </summary>
            public void Init()
            {
                HostCardDeck = new CardDeck();
                HostSecret = new List<string>();
                GuestCardDeck = new CardDeck();
                GuestSecret = new List<string>();
            }
        }

        public string GetGameInfo()
        {
            throw new NotImplementedException();
        }
    }
}
