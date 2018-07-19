using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MyWarServer.Controller;
using MyWarCommon;

namespace MyWarServer.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket serverSocket;
        private List<Client> clientList = new List<Client>();
        private ControllerManager controllerManager; // 持有controller

        public Server()
        {
            controllerManager = new ControllerManager(this);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        public Server(string ip, int port)
        {
            SetIPEndPoint(ip, port);
        }
        /// <summary>
        /// 设置IPEndPoint
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口号</param>
        public void SetIPEndPoint(string ip, int port)
        {
            controllerManager = new ControllerManager(this);
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
            Socket clientSocket = serverSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clientList.Add(client); // 持有所有client类
            Console.WriteLine("接受到客户端：" + clientSocket.RemoteEndPoint);
        }
        public void RemoveClient(Client client)
        {
            // 加锁防止多个客户端同时关闭导致数据出错
            lock (clientList)
            {
                clientList.Remove(client);
            }
        }
        /// <summary>
        /// 发送响应
        /// </summary>
        /// <param name="requestCode">请求code</param>
        /// <param name="data">数据</param>
        /// <param name="client">客户端</param>
        public void SendResponse(ActionCode actionCode, string data, Client client)
        {
            client.Send(actionCode, data);
        }
        /// <summary>
        /// 处理请求
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        /// <param name="client"></param>
        public void RequestHandle(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.RequestHandle(requestCode, actionCode, data, client);
        }
    }
}
