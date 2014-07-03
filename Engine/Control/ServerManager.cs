using Engine.Card;
using Engine.Client;
using Engine.Server;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Control
{
    /// <summary>
    /// 游戏状态(如果考虑到同时有多个游戏，必须为非静态)
    /// </summary>
    public class ServerManager
    {
        /// <summary>
        /// 游戏编号
        /// </summary>
        public int GameId = 1;
        /// <summary>
        /// 当前是否为先手回合
        /// </summary>
        public Boolean 上下半局 = true;
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
        /// 事件处理组件
        /// </summary>
        public Engine.Client.BattleEventHandler 事件处理组件 = new Engine.Client.BattleEventHandler();
        /// <summary>
        /// 主机信息
        /// </summary>
        public ServerPlayerInfo HostStatus = new ServerPlayerInfo();
        /// <summary>
        /// 从机信息
        /// </summary>
        public ServerPlayerInfo GuestStatus = new ServerPlayerInfo();
         /// <summary>
        /// 当前是否为主机回合
        /// </summary>
        /// <returns></returns>
        public Boolean IsHostNowTurn()
        {
            if (HostAsFirst && 上下半局) return true;
            if (!HostAsFirst && !上下半局) return true;
            return false;
        }
        /// <summary>
        /// 行动集
        /// </summary>
        public List<String> ActionInfo = new List<string>(); 
        /// <summary>
        /// 建立新游戏
        /// </summary>
        /// <param name="newGameId"></param>
        public ServerManager(int newGameId, String hostNickName)
        {
            this.GameId = newGameId;
            HostStatus.NickName = hostNickName;
            //决定先后手,主机位先手概率为2/1
            HostAsFirst = (GameId % 2 == 0);
        }
        /// <summary>
        /// 设定牌堆
        /// </summary>
        /// <param name="IsHost">主机</param>
        /// <param name="cards">套牌</param>
        public CardUtility.返回值枚举 SetCardStack(Boolean IsHost, Stack<String> cards)
        {
            if ((IsHost && HostAsFirst) || (!IsHost && !HostAsFirst))
            {
                //防止单机模式的时候出现一样的卡牌，所以 + 1
                HostStatus.CardDeck.Init(cards, DateTime.Now.Millisecond * 2);
            }
            else
            {
                GuestStatus.CardDeck.Init(cards, DateTime.Now.Millisecond);
            }
            return CardUtility.返回值枚举.正常;
        }
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="IsHost"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public List<String> DrawCard(Boolean IsHost, int Count)
        {
            var targetStock = IsHost ? HostStatus.CardDeck : GuestStatus.CardDeck;
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
                if (SecretCard.IsSecretAction(actionDetail))
                {
                    //TODO:
                }
                else
                {
                    ActionInfo.Add(actionDetail);
                }
            }
            //如果是回合结束的指令的时候，翻转是否是先手回合的标志
            if (Action == ActionCode.strEndTurn) 上下半局 = !上下半局;
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
    }
}