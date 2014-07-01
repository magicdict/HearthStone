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
                        allSockets.Add(socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort ,socket);
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
                        socket.Send(Response);
                        Console.WriteLine(Request + " From " + socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort);
                        Console.WriteLine(Response + " To " + socket.ConnectionInfo.ClientIpAddress + ":" + socket.ConnectionInfo.ClientPort);
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
        public static void Stop() {
            server.Dispose();
            allSockets.Clear();
        }
    }
}
