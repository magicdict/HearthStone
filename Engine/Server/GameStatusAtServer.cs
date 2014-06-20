using Engine.Card;
using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Server
{

    /// 原本应该是服务器方法，但是为了开始测试，暂时作为客户端方法
    /// 这样的话，就可以暂时不用考虑网络通讯了
    /// <summary>
    /// 游戏状态(如果考虑到同时有多个游戏，必须为非静态)
    /// </summary>
    ///<remarks>
    /// 最低要求：双方牌堆情况
    /// 棋牌类游戏难以作弊，无需大量验证
    /// </remarks>
    public class GameStatusAtServer
    {
        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameId = 1;
        /// <summary>
        /// 主机玩家名称
        /// </summary>
        public String HostNickName = String.Empty;
        /// <summary>
        /// 非主机玩家名称
        /// </summary>
        public String GuestNickName = String.Empty;
        /// <summary>
        /// 主机作为先手
        /// </summary>
        public Boolean HostAsFirst = false;
        /// <summary>
        /// 当前是否为先手回合
        /// </summary>
        public Boolean IsFirstNowTurn = true;
        /// <summary>
        /// 当前是否为主机回合
        /// </summary>
        /// <returns></returns>
        public Boolean IsHostNowTurn()
        {
            if (HostAsFirst && IsFirstNowTurn) return true;
            if (!HostAsFirst && !IsFirstNowTurn) return true;
            return false;
        }
        /// <summary>
        /// 先手牌堆
        /// </summary>
        private CardDeck HostCardDeck = new CardDeck();
        /// <summary>
        /// 先手套牌
        /// </summary>
        private Stack<String> HostCardStack;
        /// <summary>
        /// 先手奥秘
        /// </summary>
        private List<String> HostSecret = new List<string>();
        /// <summary>
        /// 后手牌堆
        /// </summary>
        private CardDeck GuestCardDeck = new CardDeck();
        /// <summary>
        /// 后手套牌
        /// </summary>
        private Stack<String> GuestCardStack;
        /// <summary>
        /// 后手奥秘
        /// </summary>
        private List<String> GuestSecret = new List<string>();
        /// <summary>
        /// 行动集
        /// </summary>
        private List<String> ActionInfo = new List<string>();
        /// <summary>
        /// 游戏管理器
        /// </summary>
        public GameManager game;
        /// <summary>
        /// 建立新游戏
        /// </summary>
        /// <param name="newGameId"></param>
        public GameStatusAtServer(int newGameId, String hostNickName, SystemManager.GameType gameType)
        {
            this.GameId = newGameId;
            this.HostNickName = hostNickName;
            //决定先后手,主机位先手概率为2/1
            HostAsFirst = (GameId % 2 == 0);
            if (gameType == SystemManager.GameType.HTML版) game = new GameManager();
        }
        /// <summary>
        /// 设定牌堆
        /// </summary>
        /// <param name="IsHost">主机</param>
        /// <param name="cards">套牌</param>
        public CardUtility.CommandResult SetCardStack(Boolean IsHost, Stack<String> cards)
        {
            if ((IsHost && HostAsFirst) || (!IsHost && !HostAsFirst))
            {
                HostCardStack = cards;
            }
            else
            {
                GuestCardStack = cards;
            }
            //如果非主机的套牌也上传的话，可以初始化了
            if (!IsHost)
            {
                Init();
            }
            return CardUtility.CommandResult.正常;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="FirstCardStack"></param>
        /// <param name="SecondCardStack"></param>
        private void Init()
        {
            //洗牌处理
            //如果以时间随机，则两者洗牌都一样
            //前者默认，后者用GameID随机
            HostCardDeck.Init(HostCardStack, 0);
            GuestCardDeck.Init(GuestCardStack, GameId);
        }
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="IsFirst"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public List<String> DrawCard(Boolean IsFirst, int Count)
        {
            var targetStock = IsFirst ? HostCardDeck : GuestCardDeck;
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
                        HostSecret.Add(SecretCardSN);
                    }
                    else
                    {
                        GuestSecret.Add(SecretCardSN);
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
                                HostSecret.Remove(secretInfo[2]);
                            }
                            else
                            {
                                GuestSecret.Remove(secretInfo[2]);
                            }
                        }
                        else
                        {
                            //后手
                            if (secretInfo[1] == CardUtility.strMe)
                            {
                                GuestSecret.Remove(secretInfo[2]);
                            }
                            else
                            {
                                HostSecret.Remove(secretInfo[2]);
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
                if (IsFirst && GuestSecret.Count != 0)
                {
                    for (int i = 0; i < GuestSecret.Count; i++)
                    {
                        if (SecretCard.IsSecretHit(GuestSecret[i], actionDetail, false))
                        {
                            HITCardList.Add(GuestSecret[i] + Engine.Utility.CardUtility.strSplitDiffMark + actionDetail);
                        }
                    }
                }
                //检查First
                if ((!IsFirst) && HostSecret.Count != 0)
                {
                    for (int i = 0; i < HostSecret.Count; i++)
                    {
                        if (SecretCard.IsSecretHit(HostSecret[i], actionDetail, false))
                        {
                            HITCardList.Add(HostSecret[i] + Engine.Utility.CardUtility.strSplitDiffMark + actionDetail);
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