using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Engine.Server;
using System.Threading;

namespace Engine.Utility
{
    public static class WebSocket
    {
        /// <summary>
        /// 本地IP地址
        /// </summary>
        public static String LocalHost = "127.0.0.1";
        /// <summary>
        /// IP地址
        /// </summary>
        public static String strIP = LocalHost;
        /// <summary>
        /// 开启服务器
        /// </summary>
        public static void StartServer()
        {
            TcpListener server = null;
            try
            {
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse(WebSocket.LocalHost);
                server = new TcpListener(localAddr, port);
                server.Start();
                while (true)
                {
                    //对于每个请求创建一个线程，线程的参数是TcpClient对象
                    TcpClient client = server.AcceptTcpClient();
                    ParameterizedThreadStart ParStart = WebSocket.Response;
                    var t = new Thread(ParStart);
                    t.Start(client);
                }
            }
            catch (SocketException)
            {

            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                server = null;
            }
        }
        /// <summary>
        /// 响应
        /// </summary>
        /// <param name="clientObj"></param>
        public static void Response(object clientObj)
        {
            var client = clientObj as TcpClient;
            // Buffer for reading data
            var bytes = new Byte[1024];
            NetworkStream stream = client.GetStream();
            ///实际长度
            int ActualSize;
            ///
            String Request = String.Empty;
            //512Byte单位进行处理
            while ((client.Available != 0) && (ActualSize = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                Request = Encoding.ASCII.GetString(bytes, 0, ActualSize);
            }
            ServerResponse.RequestType requestType = (ServerResponse.RequestType)Enum.Parse(typeof(ServerResponse.RequestType), Request.Substring(0, 3));
            String Response = ServerResponse.ProcessRequest(Request, requestType);
            bytes = Encoding.ASCII.GetBytes(Response);
            stream.Write(bytes, 0, bytes.Length);
            client.Close();
            if (!(requestType == ServerResponse.RequestType.读取行动 && String.IsNullOrEmpty(Response)))
            {
                SystemManager.TextLog("Request :[" + requestType.ToString() + "]" + Request);
                SystemManager.TextLog("Response:[" + Response.ToString() + "]");
            }
        }
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
    }
}
