using Engine.Server;
using Engine.Utility;
using System;
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
        public static Boolean UseHandCard(String GameId, Boolean IsHost, String CardSn)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.使用手牌.GetHashCode().ToString("D3") + GameId + IsHost + CardSn;
            return WebSocket.Request(requestInfo, WebSocket.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 获得游戏状态
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static GameStatus GetGameStatus(String GameId)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.战场状态.GetHashCode().ToString("D3") + GameId;
            //return WebSocket.Request(requestInfo, WebSocket.strIP);
            return null;
        }
        /// <summary>
        /// 传送套牌
        /// </summary>
        /// <param name="NickName"></param>
        public static Boolean SendDeck(String GameId, Boolean IsHost, List<String> CardDeck)
        {
            String info = String.Empty;
            foreach (var card in CardDeck)
            {
                info += card + CardUtility.strSplitArrayMark;
            }
            info = info.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
            String requestInfo = Engine.Server.ServerResponse.RequestType.传送套牌.GetHashCode().ToString("D3") + GameId +
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse) + info;
            return WebSocket.Request(requestInfo, WebSocket.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 新建游戏
        /// </summary>
        /// <param name="NickName"></param>
        public static String CreateGame(String NickName)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.新建游戏.GetHashCode().ToString("D3") + NickName;
            return WebSocket.Request(requestInfo, WebSocket.strIP);
        }
        /// <summary>
        /// 加入游戏
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public static String JoinGame(String GameId, String NickName)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.加入游戏.GetHashCode().ToString("D3") + GameId + NickName;
            return WebSocket.Request(requestInfo, WebSocket.strIP);
        }
        /// <summary>
        /// 等待游戏列表
        /// </summary>
        /// <param name="NickName"></param>
        public static String GetWatiGameList()
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.等待游戏列表.GetHashCode().ToString("D3");
            return WebSocket.Request(requestInfo, WebSocket.strIP);
        }
        /// <summary>
        /// 确认游戏状态
        /// </summary>
        /// <param name="NickName"></param>
        public static Boolean IsGameStart(String GameId)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.游戏启动状态.GetHashCode().ToString("D3") + GameId;
            return WebSocket.Request(requestInfo, WebSocket.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 确认先后手
        /// </summary>
        /// <param name="NickName"></param>
        public static Boolean IsFirst(String GameId, Boolean IsHost)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.先后手状态.GetHashCode().ToString("D3") + GameId +
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse);
            return WebSocket.Request(requestInfo, WebSocket.strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 抽牌
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="IsFirst"></param>
        /// <param name="CardCount"></param>
        /// <returns></returns>
        public static List<String> DrawCard(String GameId, bool IsFirst, int CardCount)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.抽牌.GetHashCode().ToString("D3") + GameId +
                (IsFirst ? CardUtility.strTrue : CardUtility.strFalse) + CardCount.ToString("D1");
            List<String> CardList = new List<string>();
            foreach (var card in WebSocket.Request(requestInfo, WebSocket.strIP).Split(Engine.Utility.CardUtility.strSplitArrayMark.ToArray()))
            {
                CardList.Add(card);
            }
            return CardList;
        }
        /// <summary>
        /// 回合结束
        /// </summary>
        /// <param name="GameId"></param>
        public static void TurnEnd(String GameId)
        {
            var t = new List<String>();
            t.Add(ActionCode.strEndTurn);
            WriteAction(GameId, t);
        }
        /// <summary>
        /// 添加指令
        /// </summary>
        /// <param name="GameId"></param>
        public static void WriteAction(String GameId, List<String> Action)
        {
            String Transform = String.Empty;
            foreach (var item in Action)
            {
                Transform += item + Engine.Utility.CardUtility.strSplitArrayMark;
            }
            Transform = Transform.TrimEnd(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            String requestInfo = Engine.Server.ServerResponse.RequestType.写入行动.GetHashCode().ToString("D3") + GameId + Transform;
            WebSocket.Request(requestInfo, WebSocket.strIP);
        }
        /// <summary>
        /// 读取行动
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static String ReadAction(String GameId)
        {
            String requestInfo = Engine.Server.ServerResponse.RequestType.读取行动.GetHashCode().ToString("D3") + GameId;
            return WebSocket.Request(requestInfo, WebSocket.strIP);
        }
        /// <summary>
        /// 是否触发了奥秘
        /// </summary>
        /// <returns></returns>
        public static String IsSecretHit(String GameId, bool IsFirst, List<String> Actionlst)
        {
            String Transform = String.Empty;
            foreach (var item in Actionlst)
            {
                Transform += item + Engine.Utility.CardUtility.strSplitArrayMark;
            }
            Transform = Transform.TrimEnd(Engine.Utility.CardUtility.strSplitArrayMark.ToCharArray());
            String requestInfo = Engine.Server.ServerResponse.RequestType.奥秘判定.GetHashCode().ToString("D3") + GameId +
                (IsFirst ? CardUtility.strTrue : CardUtility.strFalse) + Transform;
            return WebSocket.Request(requestInfo, WebSocket.strIP);
        }
    }
}
