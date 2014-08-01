using Engine.Control;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Utility
{
    public static class GameManager
    {
        /// <summary>
        /// 游戏管理者
        /// </summary>
        public static ClientManager MyClientManager = new ClientManager();
        /// <summary>
        /// 游戏管理者
        /// </summary>
        public static FullServerManager MyFullServerManager;
        /// <summary>
        /// 
        /// </summary>
        public static void CreateSingleGame(List<string> CardList)
        {
            SystemManager.游戏类型 = SystemManager.GameType.单机版;
            SystemManager.游戏模式 = SystemManager.GameMode.标准;
            MyFullServerManager.HostAsFirst = (DateTime.Now.Millisecond % 2) == 0;
            MyFullServerManager.InitPlayInfo();
            var CardStackFirst = new Stack<string>();
            foreach (string card in CardList)
            {
                CardStackFirst.Push(card);
            }
            MyFullServerManager.SetCardStack(true, CardStackFirst);

            var CardStackSecond = new Stack<string>();
            foreach (string card in CardList)
            {
                CardStackSecond.Push(card);
            }
            MyFullServerManager.SetCardStack(false, CardStackSecond);
        }

        public static void CreateSingleGameDefance()
        {
            SystemManager.游戏类型 = SystemManager.GameType.单机版;
            SystemManager.游戏模式 = SystemManager.GameMode.标准;
            MyFullServerManager.HostAsFirst = true;
            MyFullServerManager.事件处理组件.事件特殊处理 += (x) =>
            {
                foreach (var item in MyFullServerManager.事件处理组件.事件池)
                {
                    if (item.触发事件类型 == CardUtility.事件类型枚举.死亡 && item.触发位置.本方对方标识 == false)
                    {
                        x.AllRole.MyPublicInfo.LifePoint++;
                    }
                }
            };
            var CardStackSecond = new Stack<string>();
            for (int i = 20; i >= 1; i--)
            {
                for (int j = 0; j < 7; j++)
                {
                    CardStackSecond.Push("M9100" + i.ToString("D2"));
                }
            }
            //不随机
            MyFullServerManager.GuestStatus.CardDeck.CardList = CardStackSecond;

            var CardStackFirst = new Stack<string>();
            for (int i = 0; i < 20; i++)
            {
                foreach (var card in CardUtility.ReadyCardDic.Keys)
                {
                    if (card.Substring(1, 1) == "0") CardStackFirst.Push(card);
                }
            }
            MyFullServerManager.SetCardStack(true, CardStackFirst);
        }
    }
}
