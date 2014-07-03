using Engine.Card;
using System;
using System.Text;

namespace Engine.Client
{
    /// <summary>
    /// CS模式下面的服务器端玩家情报
    /// </summary>
    public class ClientPlayerInfo
    {
        /// <summary>
        /// 当前卡牌
        /// </summary>
        public String PlayerNickName;
        /// <summary>
        /// 本方情报
        /// </summary>
        public PublicInfo BasicInfo = new PublicInfo();
        /// <summary>
        /// 本方情报[CS]
        /// </summary>
        public PrivateInfo SelfInfo = new PrivateInfo();
    }
    /// <summary>
    /// CS模式下面的服务器端玩家情报
    /// </summary>
    public class ServerPlayerInfo
    {
        /// <summary>
        /// 主机玩家名称
        /// </summary>
        public String NickName = String.Empty;
        /// <summary>
        /// 主机私有情报
        /// </summary>
        public PrivateInfo SelfInfo = new PrivateInfo();
        /// <summary>
        /// 先手牌堆
        /// </summary>
        public CardDeck CardDeck = new CardDeck();
    }
    /// <summary>
    /// 单机版 BS版
    /// </summary>
    public class FullPlayInfo
    {
        /// <summary>
        /// 当前卡牌
        /// </summary>
        public String NickName;
        /// <summary>
        /// 本方情报
        /// </summary>
        public PublicInfo BasicInfo = new PublicInfo();
        /// <summary>
        /// 本方情报[CS]
        /// </summary>
        public PrivateInfo SelfInfo = new PrivateInfo();
        /// <summary>
        /// 先手牌堆
        /// </summary>
        public CardDeck CardDeck = new CardDeck();
    }
}
