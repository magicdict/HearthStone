using Engine.Action;
using Engine.Client;
using Engine.Control;
using Engine.Utility;
using System.Collections.Generic;
using System;

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
        public static string GameIdFormat = "D5";
        /// <summary>
        /// 等待玩家游戏[CS]
        /// </summary>
        public static Dictionary<int, ServerManager> GameWaitGuest_CS = new Dictionary<int, ServerManager>();
        /// <summary>
        /// 进行中游戏[CS]
        /// </summary>
        public static Dictionary<int, ServerManager> GameRunning_CS = new Dictionary<int, ServerManager>();
        /// <summary>
        /// 等待玩家游戏[BS]
        /// </summary>
        public static Dictionary<int, FullServerManager> GameWaitGuest_BS = new Dictionary<int, FullServerManager>();
        /// <summary>
        /// 进行中游戏[BS]
        /// </summary>
        public static Dictionary<int, FullServerManager> GameRunning_BS = new Dictionary<int, FullServerManager>();
        /// <summary>
        /// 新建游戏[CS]
        /// </summary>
        /// <returns></returns>
        public static int CreateNewGame_CS(string HostNickName)
        {
            GameId++;
            //新建游戏的同时决定游戏的先后手
            GameWaitGuest_CS.Add(GameId, new ServerManager(GameId, HostNickName));
            return GameId;
        }
        /// <summary>
        /// 新建游戏[BS]
        /// </summary>
        /// <param name="HostNickName"></param>
        /// <returns></returns>
        public static int CreateNewGame_BS(string HostNickName)
        {
            GameId++;
            //新建游戏的同时决定游戏的先后手
            var server = new FullServerManager(GameId, HostNickName);
            GameWaitGuest_BS.Add(GameId, server);
            return GameId;
        }
        /// <summary>
        /// 加入游戏[CS]
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="GuestNickName"></param>
        /// <returns> -1 表示失败</returns>
        public static int JoinGame_CS(int GameId, string GuestNickName)
        {
            if (GameWaitGuest_CS.ContainsKey(GameId))
            {
                GameWaitGuest_CS[GameId].GuestStatus.NickName = GuestNickName;
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
        public static int JoinGame_BS(int GameId, string GuestNickName)
        {
            if (GameWaitGuest_BS.ContainsKey(GameId))
            {
                GameRunning_BS.Add(GameId, GameWaitGuest_BS[GameId]);
                GameRunning_BS[GameId].GuestStatus.NickName = GuestNickName;
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
            string WaitGame = string.Empty;
            foreach (var item in GameWaitGuest_CS)
            {
                WaitGame += item.Key + "(" + item.Value.HostStatus.NickName + ")|";
            }
            WaitGame = WaitGame.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
            return WaitGame;
        }
        /// <summary>
        /// 游戏是否启动
        /// </summary>
        /// <returns></returns>
        public static string IsGameStart(int GameId)
        {
            return GameRunning_CS.ContainsKey(GameId) ? CardUtility.strTrue : CardUtility.strFalse;
        }
        /// <summary>
        /// 是否为先手
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <returns></returns>
        public static bool IsFirst(int GameId, bool IsHost)
        {
            return ((IsHost && GameRunning_CS[GameId].HostAsFirst) || (!IsHost && !GameRunning_CS[GameId].HostAsFirst));
        }
        /// <summary>
        /// 向服务器发送套牌
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="card"></param>
        public static void SetCardStack(int GameId, bool IsHost, Stack<string> card)
        {
            //IsHost == false 的时候，初始化已经完成，
            //网络版的时候，要向两个客户端发送开始游戏的下一步指令
            if (IsHost)
            {
                if (SystemManager.游戏类型 == SystemManager.GameType.客户端服务器版)
                {
                    GameWaitGuest_CS[GameId].SetCardStack(IsHost, card);
                }
                else
                {
                    GameWaitGuest_BS[GameId].SetCardStack(IsHost, card);
                }
            }
            else
            {
                if (SystemManager.游戏类型 == SystemManager.GameType.客户端服务器版)
                {
                    GameRunning_CS[GameId].SetCardStack(IsHost, card);
                }
                else
                {
                    GameRunning_BS[GameId].SetCardStack(IsHost, card);
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
        public static List<string> DrawCard(int GameId, bool IsHost, int Count)
        {
            if (SystemManager.游戏类型 == SystemManager.GameType.客户端服务器版)
            {
                return GameRunning_CS[GameId].DrawCard(IsHost, Count);
            }
            else
            {
                return GameRunning_BS[GameId].DrawCard(IsHost, Count);
            }
        }
        /// <summary>
        /// 写入动作
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Action"></param>
        public static void WriteAction(int GameId, string Action)
        {
            GameRunning_CS[GameId].WriteAction(Action);
        }
        /// <summary>
        /// 读取动作
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Action"></param>
        public static string ReadAction(int GameId)
        {
            return GameRunning_CS[GameId].ReadAction();
        }
        /// <summary>
        /// 奥秘判定
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static string SecretHit(int GameId, bool IsFirst, string ActionList)
        {
            return string.Empty;
            //return GameRunning_CS[GameId].SecretHitCheck(ActionList, IsFirst);
        }
        /// <summary>
        /// 使用手牌
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static FullServerManager.Interrupt UseHandCard(int GameId, bool IsHost, string CardSn, int Step, string SessionData)
        {
            var gamestatus = GameRunning_BS[GameId].gameStatus(IsHost);
            gamestatus.Interrupt.IsHost = IsHost;
            gamestatus.Interrupt.GameId = GameId.ToString(GameIdFormat);
            gamestatus.GameId = GameId;
            gamestatus.Interrupt.Step = Step;
            gamestatus.Interrupt.SessionData = SessionData;
            RunAction.StartAction(gamestatus, CardSn);
            if (gamestatus.Interrupt.ActionName == CardUtility.strOK)
            {
                gamestatus.AllRole.MyPublicInfo.crystal.CurrentRemainPoint -= CardUtility.GetCardInfoBySN(CardSn).使用成本;
                gamestatus.AllRole.MyPublicInfo.HandCardCount--;
                gamestatus.AllRole.MyPrivateInfo.RemoveUsedCard(CardSn);
            }
            gamestatus.Interrupt.ActionCard = new MinimizeBattleInfo.HandCardInfo();
            gamestatus.Interrupt.ActionCard.Init(CardUtility.GetCardInfoBySN(CardSn));
            return gamestatus.Interrupt;
        }
        /// <summary>
        /// 可攻击目标列表的取得
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <returns></returns>
        public static string GetFightTargetList(int GameId, bool IsHost)
        {
            var gamestatus = GameRunning_BS[GameId].gameStatus(IsHost);
            SelectUtility.SetTargetSelectEnable(SelectUtility.GetFightSelectOpt(), gamestatus);
            return SelectUtility.GetTargetListString(gamestatus);
        }
        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <param name="MyPos"></param>
        /// <param name="YourPos"></param>
        public static void Fight(int GameId, bool IsHost,int MyPos,int YourPos)
        {
            var gamestatus = GameRunning_BS[GameId].gameStatus(IsHost);
            RunAction.Fight(gamestatus, MyPos, YourPos, true);
        }
        /// <summary>
        /// 获得AI的动作
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public static FullServerManager.Interrupt GetAIAction(int gameId)
        {
            var gamestatus = GameRunning_BS[GameId].gameStatus(false);
            AI.DoAction.GetAction(gamestatus);
            return gamestatus.Interrupt;
        }
    }
}
