using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MyWarCommon;

namespace MyWarServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message message = new Message();

        public Client() { }
        public Client(Socket clientSocket, Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
        }
        /// <summary>
        /// 启动客户端接受消息
        /// </summary>
        public void Start()
        {
            clientSocket.BeginReceive(message.Data, message.StartIndex, message.RemainSize, SocketFlags.None, BeginReceive, null);
        }
        private void BeginReceive(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                // 读取数据
                message.ReadMessage(count, OnProcessMessage);
                // 循环接受消息
                Start();
            }
            catch (Exception e)
            {
                Close();
                Console.WriteLine("客户端异常：" + e);
            }

        }
        /// <summary>
        /// 接受处理消息事件
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        public void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            // 交给server处理
            server.RequestHandle(requestCode, actionCode, data, this);
        }
        /// <summary>
        /// 发送消息到客户端
        /// </summary>
        /// <param name="requestCode"></param>
        /// <param name="data"></param>
        public void Send(RequestCode requestCode, string data)
        {
            byte[] pickData = Message.PickResponseData(requestCode, data);
            // 发送数据
            clientSocket.Send(pickData);
        }
        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        public void Close()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
        }
    }
}
