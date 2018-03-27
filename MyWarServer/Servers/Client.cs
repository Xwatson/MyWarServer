using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

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
                message.ReadMessage(count);
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
