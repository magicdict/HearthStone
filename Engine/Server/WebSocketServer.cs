using Engine.Control;
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
        public static Dictionary<String, IWebSocketConnection> allSockets = new Dictionary<String, IWebSocketConnection>();
        /// <summary>
        /// Game List
        /// Key:GameID + IsHost Value:Connection
        /// </summary>
        public static Dictionary<String, String> allGames = new Dictionary<String, String>();
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
                    String MyConn = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                    socket.OnOpen = () =>
                    {
                        Console.WriteLine("Open!");
                        allSockets.Add(MyConn, socket);
                    };
                    socket.OnClose = () =>
                    {
                        Console.WriteLine("Close!");
                        allSockets.Remove(MyConn);
                    };
                    socket.OnMessage = Request =>
                    {
                        Console.WriteLine(Request + " From " + MyConn);
                        ServerResponse.RequestType requestType = (ServerResponse.RequestType)Enum.Parse(typeof(ServerResponse.RequestType), Request.Substring(0, 3));
                        String Response = ServerResponse.ProcessRequest(Request, requestType);
                        String GameId = String.Empty;
                        requestType = (ServerResponse.RequestType)Enum.Parse(typeof(ServerResponse.RequestType), Response.Substring(0, 3));
                        switch (requestType)
                        {
                            case ServerResponse.RequestType.开始游戏:
                                socket.Send(Response);
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
                                    //使用动作完成
                                    SendToBoth(GameId, Response.Substring(0,3) + Response.Substring(5));
                                }
                                else
                                {
                                    //需要后续操作
                                    Console.WriteLine(Response + " To " + MyConn);
                                    socket.Send(Response);
                                }
                                break;
                            default:
                                Console.WriteLine(Response + " To " + MyConn);
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
        public static void SendToBoth(String GameId, String Response)
        {
            var MyConn = allGames[GameId + CardUtility.strTrue];
            var MySocket = allSockets[MyConn];
            MySocket.Send(Response);
            Console.WriteLine(Response + " To " + MyConn);
            MyConn = allGames[GameId + CardUtility.strFalse];
            MySocket = allSockets[MyConn];
            MySocket.Send(Response);
            Console.WriteLine(Response + " To " + MyConn);
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
