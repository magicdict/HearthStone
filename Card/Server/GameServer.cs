using Card.Client;
using System;
using System.Collections.Generic;

namespace Card.Server
{
    public static class GameServer
    {
        /// <summary>
        /// GameId
        /// </summary>
        public static int GameId = 0;
        /// <summary>
        /// GameId Format
        /// </summary>
        public static String GameIdFormat = "D5";
        /// <summary>
        /// 等待玩家游戏
        /// </summary>
        public static Dictionary<int, GameStatusAtServer> GameWaitGuest = new Dictionary<int, GameStatusAtServer>();
        /// <summary>
        /// 进行中游戏
        /// </summary>
        public static Dictionary<int, GameStatusAtServer> GameRunning = new Dictionary<int, GameStatusAtServer>();
        /// <summary>
        /// 新建游戏
        /// </summary>
        /// <returns></returns>
        public static int CreateNewGame(String hostNickName)
        {
            GameId++;
            //新建游戏的同时决定游戏的先后手
            GameWaitGuest.Add(GameId, new GameStatusAtServer(GameId, hostNickName));
            return GameId;
        }
        /// <summary>
        /// 加入游戏
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="GuestNickName"></param>
        /// <returns> -1 表示失败</returns>
        public static int JoinGame(int GameId, String GuestNickName)
        {
            if (GameWaitGuest.ContainsKey(GameId))
            {
                GameWaitGuest[GameId].GuestNickName = GuestNickName;
                GameRunning.Add(GameId, GameWaitGuest[GameId]);
                GameWaitGuest.Remove(GameId);
                //套牌
                //GameRunning[GameId].SetCardStack(true, CardDeck.GetRandomCardStack(0));
                //GameRunning[GameId].SetCardStack(false, CardDeck.GetRandomCardStack(1));
                return GameId;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 获得等待游戏列表
        /// </summary>
        /// <returns></returns>
        public static string GetWaitGameList()
        {
            String WaitGame = String.Empty;
            foreach (var item in GameWaitGuest)
            {
                WaitGame += item.Key + "(" + item.Value.HostNickName + ")|";
            }
            WaitGame = WaitGame.TrimEnd(Card.CardUtility.strSplitArrayMark.ToCharArray());
            return WaitGame;
        }
        /// <summary>
        /// 游戏是否启动
        /// </summary>
        /// <returns></returns>
        public static String IsGameStart(int GameId)
        {
            return GameRunning.ContainsKey(GameId) ? CardUtility.strTrue : CardUtility.strFalse;
        }
        /// <summary>
        /// 是否为先手
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <returns></returns>
        public static Boolean IsFirst(int GameId, bool IsHost)
        {
            return ((IsHost && GameRunning[GameId].HostAsFirst) || (!IsHost && !GameRunning[GameId].HostAsFirst));
        }
        /// <summary>
        /// 向服务器发送套牌
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="card"></param>
        public static void SetCardStack(int GameId, Boolean IsHost, Stack<String> card)
        {
            //IsHost == false 的时候，初始化已经完成，
            //网络版的时候，要向两个客户端发送开始游戏的下一步指令
            if (IsHost)
            {
                GameWaitGuest[GameId].SetCardStack(IsHost, card);
            }
            else
            {
                GameRunning[GameId].SetCardStack(IsHost, card);
            }
        }
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsFirst"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static List<string> DrawCard(int GameId, bool IsFirst, int Count)
        {
            return GameRunning[GameId].DrawCard(IsFirst, Count);
        }
        /// <summary>
        /// 写入动作
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Action"></param>
        public static void WriteAction(int GameId, String Action)
        {
            GameRunning[GameId].WriteAction(Action);
        }
        /// <summary>
        /// 读取动作
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Action"></param>
        public static String ReadAction(int GameId)
        {
            return GameRunning[GameId].ReadAction();
        }
        /// <summary>
        /// 奥秘判定
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static String SecretHit(int GameId, bool IsFirst, String ActionList)
        {
            return GameRunning[GameId].SecretHitCheck(ActionList, IsFirst);
        }
    }
}
