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
        /// Key:Connection Value:GameID
        /// </summary>
        public static Dictionary<String, String> allGames = new Dictionary<String, String>();
        /// <summary>
        /// 获得WebSocket
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static List<IWebSocketConnection> GetConnByGameId(String GameId)
        {
            List<IWebSocketConnection> HostGuestConnection = new List<IWebSocketConnection>();
            foreach (var item in allGames)
            {
                if (item.Value == GameId) HostGuestConnection.Add(allSockets[item.Key]);
            }
            return HostGuestConnection;
        }
        /// <summary>
        /// 获得WebSocket
        /// </summary>
        /// <param name="GameId"></param>
        /// <returns></returns>
        public static IWebSocketConnection GetOppoConnByGameId(String MyConn, String GameId)
        {
            String GuestConnection = String.Empty;
            foreach (var item in allGames)
            {
                if (item.Value == GameId && item.Key != MyConn) GuestConnection = item.Key;
            }
            return allSockets[GuestConnection];
        }
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
                    socket.OnOpen = () =>
                    {
                        Console.WriteLine("Open!");
                        allSockets.Add(socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort, socket);
                    };
                    socket.OnClose = () =>
                    {
                        Console.WriteLine("Close!");
                        allSockets.Remove(socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort);
                    };
                    socket.OnMessage = Request =>
                    {
                        ServerResponse.RequestType requestType = (ServerResponse.RequestType)Enum.Parse(typeof(ServerResponse.RequestType), Request.Substring(0, 3));
                        String Response = ServerResponse.ProcessRequest(Request, requestType);
                        String GameId = String.Empty;
                        String MyConn = socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort;
                        Console.WriteLine(Request + " From " + MyConn);
                        socket.Send(Response);
                        Console.WriteLine(Response + " To " + MyConn);
                        switch (requestType)
                        {
                            case ServerResponse.RequestType.开始游戏:
                                allGames.Add(MyConn, Response.Substring(0, 5));
                                break;
                            case ServerResponse.RequestType.初始化状态:
                                //初始化状态是后手发起的，结果需要推送给双方
                                GameId = Request.Substring(3, 5);
                                GetOppoConnByGameId(MyConn, GameId).Send(Response);
                                Console.WriteLine(Response + " To OPPO");
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
        /// 关闭服务器
        /// </summary>
        public static void Stop()
        {
            server.Dispose();
            allSockets.Clear();
        }
    }
}
