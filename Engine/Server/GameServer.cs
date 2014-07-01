using Engine.Client;
using Engine.Utility;
using System;
using System.Collections.Generic;

namespace Engine.Server
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
        /// 等待玩家游戏[CS]
        /// </summary>
        public static Dictionary<int, RemoteGameManager> GameWaitGuest_CS = new Dictionary<int, RemoteGameManager>();
        /// <summary>
        /// 进行中游戏[CS]
        /// </summary>
        public static Dictionary<int, RemoteGameManager> GameRunning_CS = new Dictionary<int, RemoteGameManager>();
        /// <summary>
        /// 等待玩家游戏[BS]
        /// </summary>
        public static Dictionary<int, GameManager> GameWaitGuest_BS = new Dictionary<int, GameManager>();
        /// <summary>
        /// 进行中游戏[BS]
        /// </summary>
        public static Dictionary<int, GameManager> GameRunning_BS = new Dictionary<int, GameManager>();
        /// <summary>
        /// 新建游戏[CS]
        /// </summary>
        /// <returns></returns>
        public static int CreateNewGame_CS(String hostNickName)
        {
            GameId++;
            //新建游戏的同时决定游戏的先后手
            GameWaitGuest_CS.Add(GameId, new RemoteGameManager(GameId, hostNickName, SystemManager.CurrentGameType));
            return GameId;
        }
        /// <summary>
        /// 新建游戏[BS]
        /// </summary>
        /// <param name="HostNickName"></param>
        /// <returns></returns>
        public static int CreateNewGame_BS(String HostNickName)
        {
            GameId++;
            //新建游戏的同时决定游戏的先后手
            GameWaitGuest_BS.Add(GameId, new GameManager() { 
                GameId = GameId, 
                SimulateServer = new RemoteGameManager(GameId, HostNickName, SystemManager.GameType.HTML版) 
            });
            GameWaitGuest_BS[GameId].SimulateServer.serverinfo.HostNickName = HostNickName;
            return GameId;
        }
        /// <summary>
        /// 加入游戏[CS]
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="GuestNickName"></param>
        /// <returns> -1 表示失败</returns>
        public static int JoinGame_CS(int GameId, String GuestNickName)
        {
            if (GameWaitGuest_CS.ContainsKey(GameId))
            {
                GameWaitGuest_CS[GameId].serverinfo.GuestNickName = GuestNickName;
                GameRunning_CS.Add(GameId, GameWaitGuest_CS[GameId]);
                GameWaitGuest_CS.Remove(GameId);
                return GameId;
            }
            else
            {
                return -1;
            }
        }
        /// <summary>
        /// 加入游戏[BS]
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="GuestNickName"></param>
        /// <returns> -1 表示失败</returns>
        public static int JoinGame_BS(int GameId, String GuestNickName)
        {
            if (GameWaitGuest_BS.ContainsKey(GameId))
            {
                GameRunning_BS.Add(GameId, GameWaitGuest_BS[GameId]);
                GameRunning_BS[GameId].SimulateServer.serverinfo.GuestNickName = GuestNickName;
                GameWaitGuest_BS.Remove(GameId);
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
            foreach (var item in GameWaitGuest_CS)
            {
                WaitGame += item.Key + "(" + item.Value.serverinfo.HostNickName + ")|";
            }
            WaitGame = WaitGame.TrimEnd(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            return WaitGame;
        }
        /// <summary>
        /// 游戏是否启动
        /// </summary>
        /// <returns></returns>
        public static String IsGameStart(int GameId)
        {
            return GameRunning_CS.ContainsKey(GameId) ? CardUtility.strTrue : CardUtility.strFalse;
        }
        /// <summary>
        /// 是否为先手
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <returns></returns>
        public static Boolean IsFirst(int GameId, bool IsHost)
        {
            return ((IsHost && GameRunning_CS[GameId].serverinfo.HostAsFirst) || (!IsHost && !GameRunning_CS[GameId].serverinfo.HostAsFirst));
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
                if (SystemManager.CurrentGameType == SystemManager.GameType.客户端服务器版)
                {
                    GameWaitGuest_CS[GameId].SetCardStack(IsHost, card);
                }
                else
                {
                    GameWaitGuest_BS[GameId].SimulateServer.SetCardStack(IsHost, card);
                }
            }
            else
            {
                if (SystemManager.CurrentGameType == SystemManager.GameType.客户端服务器版)
                {
                    GameRunning_CS[GameId].SetCardStack(IsHost, card);
                }
                else
                {
                    GameRunning_BS[GameId].SimulateServer.SetCardStack(IsHost, card);
                }
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
            return GameRunning_CS[GameId].DrawCard(IsFirst, Count);
        }
        /// <summary>
        /// 写入动作
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Action"></param>
        public static void WriteAction(int GameId, String Action)
        {
            GameRunning_CS[GameId].WriteAction(Action);
        }
        /// <summary>
        /// 读取动作
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Action"></param>
        public static String ReadAction(int GameId)
        {
            return GameRunning_CS[GameId].ReadAction();
        }
        /// <summary>
        /// 奥秘判定
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static String SecretHit(int GameId, bool IsFirst, String ActionList)
        {
            return GameRunning_CS[GameId].SecretHitCheck(ActionList, IsFirst);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static string UseHandCard(int GameId, bool IsHost, string CardSn)
        {
            var result = GameRunning_CS[GameId].UseHandCard(IsHost, CardSn);
            if (result == CardUtility.返回值枚举.正常)
            {
                return CardUtility.strTrue;
            }
            else
            {
                return CardUtility.strFalse;
            }
        }
    }
}
