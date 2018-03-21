using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MyWarServer.Server
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;

        public Server() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        public Server(string ip, int port)
        {
            SetIPEndPoint(ip, port);
            Start();
        }
        /// <summary>
        /// 设置IPEndPoint
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        public void SetIPEndPoint(string ip, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }
        /// <summary>
        /// 启动服务端
        /// </summary>
        public void Start()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(ipEndPoint);
            serverSocket.Listen(0);

            serverSocket.BeginAccept(BeginAcceptCallBack, null);
        }
        // 异步接受客户端
        private void BeginAcceptCallBack(IAsyncResult ar)
        {

        }
    }
}
