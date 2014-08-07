using Engine.Server;
using Fleck;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Engine.Utility
{
    public static class WebSocketServer
    {
        /// <summary>
        /// Server
        /// </summary>
        public static Fleck.WebSocketServer server = new Fleck.WebSocketServer("ws://0.0.0.0:13001");
        /// <summary>
        /// Socket List
        /// Key:Connection Value:Socket
        /// </summary>
        public static Dictionary<string, IWebSocketConnection> allSockets = new Dictionary<string, IWebSocketConnection>();
        /// <summary>
        /// Game List
        /// Key:GameID + IsHost Value:Connection
        /// </summary>
        public static Dictionary<string, string> allGames = new Dictionary<string, string>();
         /// <summary>
        /// 开启服务器
        /// </summary>
        public static void Start()
        {
            try
            {
                FleckLog.Level = LogLevel.Error;
                server.Start(socket =>
                {
                    string MyConn = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    socket.OnOpen = () =>
                    {
                        //Console.WriteLine("Open!");
                        SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Open Connect", IP = MyConn, logTime = DateTime.Now });
                        allSockets.Add(MyConn, socket);
                    };
                    socket.OnClose = () =>
                    {
                        //Console.WriteLine("Close!");
                        SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Close Connect", IP = MyConn, logTime = DateTime.Now });
                        allSockets.Remove(MyConn);
                    };
                    socket.OnMessage = Request =>
                    {
                        SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Request：" + Request, IP = MyConn, logTime = DateTime.Now });
                        ServerResponse.RequestType requestType = (ServerResponse.RequestType)Enum.Parse(typeof(ServerResponse.RequestType), Request.Substring(0, 3));
                        string Response = ServerResponse.ProcessRequest(Request, requestType);
                        string GameId = string.Empty;
                        requestType = (ServerResponse.RequestType)Enum.Parse(typeof(ServerResponse.RequestType), Response.Substring(0, 3));
                        switch (requestType)
                        {
                            case ServerResponse.RequestType.开始游戏:
                            case ServerResponse.RequestType.开始单机游戏:
                                socket.Send(Response);
                                // Key:GameID + IsHost Value:Connection
                                allGames.Add(Response.Substring(3, 6), MyConn);
                                break;
                            case ServerResponse.RequestType.初始化状态:
                            case ServerResponse.RequestType.回合结束:
                            case ServerResponse.RequestType.攻击行为:
                                //初始化状态是后手发起的，结果需要推送给双方
                                GameId = Request.Substring(3, 5);
                                SendToBoth(GameId, Response);
                                break;
                            case ServerResponse.RequestType.使用手牌:
                                GameId = Request.Substring(3, 5);
                                if (Response.Substring(3,2) == CardUtility.strOK)
                                {
                                    //使用动作完成后的战场状态
                                    SendToBoth(GameId, Response.Substring(0,3) + Response.Substring(5));
                                }
                                else
                                {
                                    //需要后续操作，中断续行
                                    SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Response：" + Response, IP = MyConn, logTime = DateTime.Now });
                                    socket.Send(Response);
                                }
                                break;
                            case ServerResponse.RequestType.结束游戏:
                                GameId = Request.Substring(3, 5);
                                //游戏字典去除，但是链接没有结束
                                // Key:GameID + IsHost Value:Connection
                                allGames.Remove(Request.Substring(3, 6));
                                bool CanRemove = true;
                                foreach (var item in allGames.Keys)
                                {
                                    if (item.StartsWith(GameId))
                                    {
                                        CanRemove = false;
                                        break;
                                    }
                                }
                                if (CanRemove) ServerResponse.RemoveGame(GameId);
                                break;
                            default:
                                SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Response：" + Response, IP = MyConn, logTime = DateTime.Now });
                                socket.Send(Response);
                                break;
                        }
                    };
                });
            }
            catch (SocketException)
            {

            }
            finally
            {
                // Stop listening for new clients.
            }
        }
        /// <summary>
        /// 发送给双方
        /// </summary>
        /// <param name="GameId"></param>
        /// <param name="Response"></param>
        public static void SendToBoth(string GameId, string Response)
        {
            var MyConn = allGames[GameId + CardUtility.strTrue];
            var MySocket = allSockets[MyConn];
            MySocket.Send(Response);
            SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Response：" + Response, IP = MyConn, logTime = DateTime.Now });
            if (allGames.ContainsKey(GameId + CardUtility.strFalse))
            {
                //如果不是冒险模式
                MyConn = allGames[GameId + CardUtility.strFalse];
                MySocket = allSockets[MyConn];
                MySocket.Send(Response);
                SystemManager.Logger(new CSharpUtility.LogRec() { Info = "Response：" + Response, IP = MyConn, logTime = DateTime.Now });
            }
        }
        /// <summary>
        /// 关闭服务器
        /// </summary>
        public static void Stop()
        {
            server.Dispose();
            allSockets.Clear();
        }
    }
}
