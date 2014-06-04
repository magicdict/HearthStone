using Card.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Card.Client
{
    public static class ClientRequest
    {
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <param name="strIP"></param>
        /// <returns></returns>
        public static String Request(String requestInfo, String strIP)
        {
            TcpClient client = new TcpClient();
            IPAddress localAddr = IPAddress.Parse(strIP);
            client.Connect(localAddr, 13000);
            var stream = client.GetStream();
            var bytes = new Byte[1024];
            bytes = Encoding.ASCII.GetBytes(requestInfo);
            stream.Write(bytes, 0, bytes.Length);
            String Response = String.Empty;
            using (StreamReader reader = new StreamReader(stream))
            {
                while (reader.Peek() != -1)
                {
                    Response = reader.ReadLine();
                }
            }
            client.Close();
            return Response;
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public static String strIP = "127.0.0.1";
        /// <summary>
        /// 传送套牌
        /// </summary>
        /// <param name="NickName"></param>
        public static Boolean SendDeck(int GameId, Boolean IsHost, List<String> CardDeck)
        {
            String info = String.Empty;
            foreach (var card in CardDeck)
            {
                info += card + CardUtility.strSplitArrayMark;
            }
            info = info.TrimEnd(CardUtility.strSplitArrayMark.ToCharArray());
            String requestInfo = Card.Server.ServerResponse.RequestType.传送套牌.GetHashCode().ToString("D3") + GameId.ToString(GameServer.GameIdFormat) +
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse) + info;
            return Request(requestInfo, strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 新建游戏
        /// </summary>
        /// <param name="NickName"></param>
        public static String CreateGame(String NickName)
        {
            String requestInfo = Card.Server.ServerResponse.RequestType.新建游戏.GetHashCode().ToString("D3") + NickName;
            return Request(requestInfo, strIP);
        }
        /// <summary>
        /// 加入游戏
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="NickName"></param>
        /// <returns></returns>
        public static String JoinGame(int GameId, String NickName)
        {
            String requestInfo = Card.Server.ServerResponse.RequestType.加入游戏.GetHashCode().ToString("D3") + GameId.ToString(GameServer.GameIdFormat) + NickName;
            return Request(requestInfo, strIP);
        }
        /// <summary>
        /// 等待游戏列表
        /// </summary>
        /// <param name="NickName"></param>
        public static String GetWatiGameList()
        {
            String requestInfo = Card.Server.ServerResponse.RequestType.等待游戏列表.GetHashCode().ToString("D3");
            return Request(requestInfo, strIP);
        }
        /// <summary>
        /// 确认游戏状态
        /// </summary>
        /// <param name="NickName"></param>
        public static Boolean IsGameStart(String GameId)
        {
            String requestInfo = Card.Server.ServerResponse.RequestType.游戏启动状态.GetHashCode().ToString("D3") + GameId;
            return Request(requestInfo, strIP) == CardUtility.strTrue;
        }
        /// <summary>
        /// 确认先后手
        /// </summary>
        /// <param name="NickName"></param>
        public static Boolean IsFirst(String GameId, Boolean IsHost)
        {
            String requestInfo = Card.Server.ServerResponse.RequestType.先后手状态.GetHashCode().ToString("D3") + GameId + 
                (IsHost ? CardUtility.strTrue : CardUtility.strFalse);
            return Request(requestInfo, strIP) == CardUtility.strTrue;
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
            String requestInfo = Card.Server.ServerResponse.RequestType.抽牌.GetHashCode().ToString("D3") + GameId + 
                (IsFirst ? CardUtility.strTrue : CardUtility.strFalse) + CardCount.ToString("D1");
            List<String> CardList = new List<string>();
            foreach (var card in Request(requestInfo, strIP).Split(Card.CardUtility.strSplitArrayMark.ToArray()))
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
            var t =  new List<String>();
            t.Add(ActionCode.strEndTurn);
            WriteAction(GameId,t);
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
                Transform += item + Card.CardUtility.strSplitArrayMark;
            }
            Transform = Transform.TrimEnd(Card.CardUtility.strSplitArrayMark.ToCharArray());
            String requestInfo = Card.Server.ServerResponse.RequestType.写入行动.GetHashCode().ToString("D3") + GameId + Transform;
            Request(requestInfo, strIP);
        }
        /// <summary>
        /// 读取行动
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static String ReadAction(String GameId)
        {
            String requestInfo = Card.Server.ServerResponse.RequestType.读取行动.GetHashCode().ToString("D3") + GameId;
            return Request(requestInfo, strIP);
        }
        /// <summary>
        /// 是否触发了奥秘
        /// </summary>
        /// <returns></returns>
        public static String IsSecretHit(String GameId, bool IsFirst,List<String> Actionlst)
        {
            String Transform = String.Empty;
            foreach (var item in Actionlst)
            {
                Transform += item + Card.CardUtility.strSplitArrayMark;
            }
            Transform = Transform.TrimEnd(Card.CardUtility.strSplitArrayMark.ToCharArray());
            String requestInfo = Card.Server.ServerResponse.RequestType.奥秘判定.GetHashCode().ToString("D3") + GameId +
                (IsFirst ? CardUtility.strTrue : CardUtility.strFalse) + Transform;
            return Request(requestInfo, strIP);
        }
    }
}
