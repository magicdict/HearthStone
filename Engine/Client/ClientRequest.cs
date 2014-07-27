using Engine.Server;
using Engine.Utility;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Client
{
    public static class ClientRequest
    {
        /// <summary>
        /// 使用手牌
        /// </summary>
        /// <param name="GameID">游戏ID</param>
        /// <param name="IsHost">是否为主机</param>
        /// <param name="CardSn">卡牌序列号</param>
        /// <returns></returns>
        public static bool UseHandCard(string GameId, bool IsHost, string CardSn)
        {
            string requestInfo = ServerResponse.RequestType.使用手牌.GetHashCode().ToString("D3") + GameId + IsHost + CardSn;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 获得游戏状态
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static ClientPlayerInfo GetGameStatus(string GameId)
        {
            string requestInfo = ServerResponse.RequestType.战场状态.GetHashCode().ToString("D3") + GameId;
            //return WebSocket.Request(requestInfo, WebSocket.strIP);
            return null;
        }
        /// <summary>
        /// 传送套牌
        /// </summary>
        /// <param name="NickName"></param>
        public static bool SendDeck(string GameId, bool IsHost, List<string> CardDeck)
        {
            string info = string.Empty;
            foreach (var card in CardDeck)
            {
                info += card + CardUtility.strSplitArrayMark;
            }
            info = info.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
            string requestInfo = ServerResponse.RequestType.传送套牌.GetHashCode().ToString("D3") + GameId +
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse) + info;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 新建游戏
        /// </summary>
        /// <param name="NickName"></param>
        public static string CreateGame(string NickName)
        {
            string requestInfo = ServerResponse.RequestType.新建游戏.GetHashCode().ToString("D3") + NickName;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP);
        }
        /// <summary>
        /// 加入游戏
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public static string JoinGame(string GameId, string NickName)
        {
            string requestInfo = ServerResponse.RequestType.加入游戏.GetHashCode().ToString("D3") + GameId + NickName;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP);
        }
        /// <summary>
        /// 等待游戏列表
        /// </summary>
        /// <param name="NickName"></param>
        public static string GetWatiGameList()
        {
            string requestInfo = ServerResponse.RequestType.等待游戏列表.GetHashCode().ToString("D3");
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP);
        }
        /// <summary>
        /// 确认游戏状态
        /// </summary>
        /// <param name="NickName"></param>
        public static bool IsGameStart(string GameId)
        {
            string requestInfo = ServerResponse.RequestType.游戏启动状态.GetHashCode().ToString("D3") + GameId;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 确认先后手
        /// </summary>
        /// <param name="NickName"></param>
        public static bool IsFirst(string GameId, bool IsHost)
        {
            string requestInfo = ServerResponse.RequestType.先后手状态.GetHashCode().ToString("D3") + GameId +
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse);
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsHost"></param>
        /// <param name="CardCount"></param>
        /// <returns></returns>
        public static List<string> DrawCard(string GameId, bool IsHost, int CardCount)
        {
            string requestInfo = ServerResponse.RequestType.抽牌.GetHashCode().ToString("D3") + GameId +
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse) + CardCount.ToString("D1");
            List<string> CardList = new List<string>();
            foreach (var card in TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP).Split(CardUtility.strSplitArrayMark.ToArray()))
            {
                CardList.Add(card);
            }
            return CardList;
        }
        /// <summary>
        /// 回合结束
        /// </summary>
        /// <param name="GameId"></param>
        public static void TurnEnd(string GameId)
        {
            var t = new List<string>();
            t.Add(ActionCode.strEndTurn);
            WriteAction(GameId, t);
        }
        /// <summary>
        /// 添加指令
        /// </summary>
        /// <param name="GameId"></param>
        public static void WriteAction(string GameId, List<string> Action)
        {
            string Transform = string.Empty;
            foreach (var item in Action)
            {
                Transform += item + CardUtility.strSplitArrayMark;
            }
            Transform = Transform.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
            string requestInfo = ServerResponse.RequestType.写入行动.GetHashCode().ToString("D3") + GameId + Transform;
            TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP);
        }
        /// <summary>
        /// 读取行动
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static string ReadAction(string GameId)
        {
            string requestInfo = ServerResponse.RequestType.读取行动.GetHashCode().ToString("D3") + GameId;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP);
        }
        /// <summary>
        /// 是否触发了奥秘
        /// </summary>
        /// <returns></returns>
        public static string IsSecretHit(string GameId, bool IsFirst, List<string> Actionlst)
        {
            string Transform = string.Empty;
            foreach (var item in Actionlst)
            {
                Transform += item + CardUtility.strSplitArrayMark;
            }
            Transform = Transform.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
            string requestInfo = ServerResponse.RequestType.奥秘判定.GetHashCode().ToString("D3") + GameId +
                (IsFirst ? CardUtility.strTrue : CardUtility.strFalse) + Transform;
            return TcpSocketServer.Request(requestInfo, TcpSocketServer.strIP);
        }
    }
}
