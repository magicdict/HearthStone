﻿using Engine.Client;
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
        /// 等待玩家游戏
        /// </summary>
        public static Dictionary<int, RemoteGameManager> GameWaitGuest = new Dictionary<int, RemoteGameManager>();
        /// <summary>
        /// 进行中游戏
        /// </summary>
        public static Dictionary<int, RemoteGameManager> GameRunning = new Dictionary<int, RemoteGameManager>();
        /// <summary>
        /// 新建游戏
        /// </summary>
        /// <returns></returns>
        public static int CreateNewGame(String hostNickName)
        {
            GameId++;
            //新建游戏的同时决定游戏的先后手
            GameWaitGuest.Add(GameId, new RemoteGameManager(GameId, hostNickName, SystemManager.CurrentGameType));
            return GameId;
        }
        /// <summary>
        /// 加入游戏
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="GuestNickName"></param>
        /// <returns> -1 表示失败</returns>
        public static CardUtility.返回值枚举 JoinGame(int GameId, String GuestNickName)
        {
            if (GameWaitGuest.ContainsKey(GameId))
            {
                GameWaitGuest[GameId].serverinfo.GuestNickName = GuestNickName;
                GameRunning.Add(GameId, GameWaitGuest[GameId]);
                GameWaitGuest.Remove(GameId);
                return CardUtility.返回值枚举.正常;
            }
            else
            {
                return CardUtility.返回值枚举.异常;
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
            return ((IsHost && GameRunning[GameId].serverinfo.HostAsFirst) || (!IsHost && !GameRunning[GameId].serverinfo.HostAsFirst));
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static string UseHandCard(int GameId, bool IsHost, string CardSn)
        {
            var result = GameRunning[GameId].UseHandCard(IsHost, CardSn);
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
