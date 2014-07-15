using Engine.Action;
using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Control
{
    /// <summary>
    /// FullServerManager
    /// </summary>
    public class FullServerManager
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
        public FullPlayInfo HostStatus = new FullPlayInfo();
        /// <summary>
        /// 从机信息
        /// </summary>
        public FullPlayInfo GuestStatus = new FullPlayInfo();
        /// <summary>
        /// 中断信息
        /// HTML页面和服务器端交互
        /// </summary>
        public struct Interrupt
        {
            /// <summary>
            /// 游戏ID
            /// </summary>
            public String GameId;
            /// <summary>
            /// 是否为主机
            /// </summary>
            public Boolean IsHost;
            /// <summary>
            /// 中断步骤
            /// </summary>
            public int Step;
            /// <summary>
            /// 中断步骤名称
            /// </summary>
            public String ActionName;
            /// <summary>
            /// 附加情报
            /// </summary>
            public String ExternalInfo;
            /// <summary>
            /// 客户端数据
            /// </summary>
            public String SessionData;
        }
        /// <summary>
        /// 当前中断
        /// </summary>
        public Interrupt CurrentInterrupt = new Interrupt();
        /// <summary>
        /// 获得动作数据包
        /// </summary>
        /// <param name="IsHost"></param>
        /// <returns></returns>
        public ActionStatus gameStatus(Boolean IsHost)
        {
            ActionStatus actionStatus = new ActionStatus()
            {
                GameId = GameId,
                battleEvenetHandler = 事件处理组件,
                IsHost = IsHost,
                Interrupt = CurrentInterrupt,
            };
            if (IsHost)
            {
                actionStatus.AllRole.MyPublicInfo = HostStatus.BasicInfo;
                actionStatus.AllRole.MyPrivateInfo = HostStatus.SelfInfo;
                actionStatus.AllRole.YourPublicInfo = GuestStatus.BasicInfo;
                actionStatus.AllRole.YourPrivateInfo = GuestStatus.SelfInfo;
            }
            else
            {
                actionStatus.AllRole.MyPublicInfo = GuestStatus.BasicInfo;
                actionStatus.AllRole.MyPrivateInfo = GuestStatus.SelfInfo;
                actionStatus.AllRole.YourPublicInfo = HostStatus.BasicInfo;
                actionStatus.AllRole.YourPrivateInfo = HostStatus.SelfInfo;
            }
            return actionStatus;
        }

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
        /// FullServerManager
        /// </summary>
        /// <param name="newGameId"></param>
        /// <param name="hostNickName"></param>
        public FullServerManager(int newGameId, String hostNickName)
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
        /// 初始化
        /// </summary>
        public void InitPlayInfo()
        {
            //位置
            HostStatus.BasicInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = true, 位置 = BattleFieldInfo.HeroPos };
            GuestStatus.BasicInfo.战场位置 = new CardUtility.指定位置结构体() { 本方对方标识 = false, 位置 = BattleFieldInfo.HeroPos };
            HostStatus.BasicInfo.BattleField.本方对方标识 = true;
            GuestStatus.BasicInfo.BattleField.本方对方标识 = false;
            //水晶
            HostStatus.BasicInfo.crystal.CurrentFullPoint = 0;
            HostStatus.BasicInfo.crystal.CurrentRemainPoint = 0;
            GuestStatus.BasicInfo.crystal.CurrentFullPoint = 0;
            GuestStatus.BasicInfo.crystal.CurrentRemainPoint = 0;
            //英雄技能：奥术飞弹
            HostStatus.BasicInfo.HeroAbility = (Engine.Card.SpellCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            GuestStatus.BasicInfo.HeroAbility = (Engine.Card.SpellCard)Engine.Utility.CardUtility.GetCardInfoBySN("A000056");
            
            //TEST START
            HostStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN("M000059"));
            //TEST END

            //初始化双方手牌
            int DrawCardCnt = 0;
            if (HostAsFirst)
            {
                DrawCardCnt = PublicInfo.BasicHandCardCount;
                foreach (var card in DrawCard(true, DrawCardCnt))
                {
                    HostStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                }
                DrawCardCnt = PublicInfo.BasicHandCardCount + 1;
                foreach (var card in DrawCard(false, DrawCardCnt))
                {
                    GuestStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                }
                GuestStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.SpellCard.SN幸运币));

                HostStatus.BasicInfo.RemainCardDeckCount = CardDeck.MaxCards - 3;
                GuestStatus.BasicInfo.RemainCardDeckCount = CardDeck.MaxCards - 4;
                HostStatus.BasicInfo.HandCardCount = PublicInfo.BasicHandCardCount;
                GuestStatus.BasicInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
                TurnStart(true);
            }
            else
            {
                DrawCardCnt = PublicInfo.BasicHandCardCount + 1;
                foreach (var card in DrawCard(true, DrawCardCnt))
                {
                    HostStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                }
                HostStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(Engine.Card.SpellCard.SN幸运币));

                DrawCardCnt = PublicInfo.BasicHandCardCount;
                foreach (var card in DrawCard(false, DrawCardCnt))
                {
                    GuestStatus.SelfInfo.handCards.Add(CardUtility.GetCardInfoBySN(card));
                }
                HostStatus.BasicInfo.RemainCardDeckCount = CardDeck.MaxCards - 4;
                GuestStatus.BasicInfo.RemainCardDeckCount = CardDeck.MaxCards - 3;
                HostStatus.BasicInfo.HandCardCount = PublicInfo.BasicHandCardCount + 1 + 1;
                GuestStatus.BasicInfo.HandCardCount = PublicInfo.BasicHandCardCount;
                TurnStart(false);
            }
        }
        /// <summary>
        /// 开始回合
        /// </summary>
        /// <param name="IsHost"></param>
        public void TurnStart(Boolean IsHost)
        {
            gameStatus(IsHost).AllRole.MyPrivateInfo.handCards.Add(CardUtility.GetCardInfoBySN(DrawCard(IsHost, 1)[0]));
            TurnAction.TurnStart(gameStatus(IsHost));
        }
        /// <summary>
        /// 结束回合
        /// </summary>
        /// <param name="IsHost"></param>
        public void TurnEnd(Boolean IsHost)
        {
            TurnAction.TurnEnd(gameStatus(IsHost));
            TurnStart(!IsHost);
        }
    }
}
