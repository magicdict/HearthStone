using Engine.Card;
using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Server
{

    /// <summary>
    /// 游戏状态(如果考虑到同时有多个游戏，必须为非静态)
    /// </summary>
    /// <remarks>
    /// 原本应该是服务器方法，但是为了开始测试，暂时作为客户端方法
    /// 这样的话，就可以暂时不用考虑网络通讯了
    /// 最低要求：双方牌堆情况
    /// 棋牌类游戏难以作弊，无需大量验证
    /// </remarks>
    public class RemoteGameManager
    {
        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameId = 1;
        /// <summary>
        /// 当前是否为先手回合
        /// </summary>
        public Boolean IsFirstNowTurn = true;
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
        /// <summary>
        /// 服务器端信息
        /// </summary>
        public ServerInfo serverinfo = new ServerInfo();
        /// <summary>
        /// 当前是否为主机回合
        /// </summary>
        /// <returns></returns>
        public Boolean IsHostNowTurn()
        {
            if (serverinfo.HostAsFirst && IsFirstNowTurn) return true;
            if (!serverinfo.HostAsFirst && !IsFirstNowTurn) return true;
            return false;
        }
        /// <summary>
        /// 行动集
        /// </summary>
        public List<String> ActionInfo = new List<string>(); 
        /// <summary>
        /// 游戏状态容器(HTML)
        /// </summary>
        public GameStatus BSgamestatus;
        /// <summary>
        /// 建立新游戏
        /// </summary>
        /// <param name="newGameId"></param>
        public RemoteGameManager(int newGameId, String hostNickName, SystemManager.GameType gameType)
        {
            this.GameId = newGameId;
            serverinfo.HostNickName = hostNickName;
            //决定先后手,主机位先手概率为2/1
            serverinfo.HostAsFirst = (GameId % 2 == 0);
            serverinfo.Init();
            if (gameType == SystemManager.GameType.HTML版) BSgamestatus = new GameStatus();
        }
        /// <summary>
        /// 设定牌堆
        /// </summary>
        /// <param name="IsHost">主机</param>
        /// <param name="cards">套牌</param>
        public CardUtility.CommandResult SetCardStack(Boolean IsHost, Stack<String> cards)
        {
            if ((IsHost && serverinfo.HostAsFirst) || (!IsHost && !serverinfo.HostAsFirst))
            {
                //防止单机模式的时候出现一样的卡牌，所以 + 1
                serverinfo.HostCardDeck.Init(cards, DateTime.Now.Millisecond + 1);
            }
            else
            {
                serverinfo.GuestCardDeck.Init(cards, DateTime.Now.Millisecond);
            }
            return CardUtility.CommandResult.正常;
        }
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="IsHost"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public List<String> DrawCard(Boolean IsHost, int Count)
        {
            var targetStock = IsHost ? serverinfo.HostCardDeck : serverinfo.GuestCardDeck;
            return targetStock.DrawCard(Count);
        }
        /// <summary>
        /// 追加指令
        /// </summary>
        /// <param name="Action"></param>
        public void WriteAction(String Action)
        {
            foreach (var actionDetail in Action.Split(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray()))
            {
                if (actionDetail.StartsWith(ActionCode.strSecret + CardUtility.strSplitMark))
                {
                    //使用奥秘
                    String SecretCardSN = actionDetail.Substring(ActionCode.strSecret.Length + Engine.Utility.CardUtility.strSplitMark.Length);
                    if (IsFirstNowTurn)
                    {
                        serverinfo.HostSecret.Add(SecretCardSN);
                    }
                    else
                    {
                        serverinfo.GuestSecret.Add(SecretCardSN);
                    }
                    //奥秘的时候，不放松奥秘内容
                    //注意和ActionCode.GetActionType()保持一致
                    ActionInfo.Add(ActionCode.strSecret);
                }
                else
                {
                    //奥秘判断 注意：这个动作需要改变FirstSecret和SecondSecret
                    if (actionDetail.StartsWith(ActionCode.strHitSecret))
                    {
                        var secretInfo = actionDetail.Split(CardUtility.strSplitMark.ToCharArray());
                        if (IsFirstNowTurn)
                        {
                            //先手
                            if (secretInfo[1] == CardUtility.strMe)
                            {
                                serverinfo.HostSecret.Remove(secretInfo[2]);
                            }
                            else
                            {
                                serverinfo.GuestSecret.Remove(secretInfo[2]);
                            }
                        }
                        else
                        {
                            //后手
                            if (secretInfo[1] == CardUtility.strMe)
                            {
                                serverinfo.GuestSecret.Remove(secretInfo[2]);
                            }
                            else
                            {
                                serverinfo.HostSecret.Remove(secretInfo[2]);
                            }
                        }
                    }
                    //动作写入
                    ActionInfo.Add(actionDetail);
                }
            }
            //如果是回合结束的指令的时候，翻转是否是先手回合的标志
            if (Action == ActionCode.strEndTurn) IsFirstNowTurn = !IsFirstNowTurn;
        }
        /// <summary>
        /// 读取指令
        /// </summary>
        public String ReadAction()
        {
            String lstAction = String.Empty;
            foreach (var item in ActionInfo)
            {
                lstAction += item + Engine.Utility.CardUtility.strSplitArrayMark;
            }
            if (!String.IsNullOrEmpty(lstAction)) lstAction = lstAction.TrimEnd(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            ActionInfo.Clear();
            return lstAction;
        }
        /// <summary>
        /// 是否HIT对方奥秘
        /// </summary>
        /// <param name="IsFirst">是否为先手</param>
        /// <returns></returns>
        public string SecretHitCheck(String Action, bool IsFirst)
        {
            //奥秘判断 注意：这个动作并不改变FirstSecret和SecondSecret
            //1.例如，发生战斗的时候，如果两个随从都死了，
            //同时两边都有随从死亡的奥秘，则整个动作序列可能触发两边的奥秘
            //<本方奥秘在客户端判断>注意方向
            //2.服务器端只做判断，并且返回命中奥秘的列表，不做任何其他操作！
            List<String> HITCardList = new List<string>();
            foreach (var actionDetail in Action.Split(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray()))
            {
                //检查Second
                if (IsFirst && serverinfo.GuestSecret.Count != 0)
                {
                    for (int i = 0; i < serverinfo.GuestSecret.Count; i++)
                    {
                        if (SecretCard.IsSecretHit(serverinfo.GuestSecret[i], actionDetail, false))
                        {
                            HITCardList.Add(serverinfo.GuestSecret[i] + Engine.Utility.CardUtility.strSplitDiffMark + actionDetail);
                        }
                    }
                }
                //检查First
                if ((!IsFirst) && serverinfo.HostSecret.Count != 0)
                {
                    for (int i = 0; i < serverinfo.HostSecret.Count; i++)
                    {
                        if (SecretCard.IsSecretHit(serverinfo.HostSecret[i], actionDetail, false))
                        {
                            HITCardList.Add(serverinfo.HostSecret[i] + Engine.Utility.CardUtility.strSplitDiffMark + actionDetail);
                        }
                    }
                }
            }
            String strRtn = String.Empty;
            if (HITCardList.Count != 0)
            {
                foreach (var card in HITCardList)
                {
                    strRtn += card + Engine.Utility.CardUtility.strSplitArrayMark;
                }
                strRtn = strRtn.TrimEnd(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            }
            return strRtn;
        }
    }
}