using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyWarServer.Servers;

namespace MyWarServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "127.0.0.1";
            int port = 3318;
            try
            {
                Server server = new Server(host, port);
                server.Start();
                Console.WriteLine("服务器启动成功，请访问："+ host + "， 端口：" + port);
            }
            catch (Exception e)
            {
                Console.WriteLine("服务启动失败：", e);
            }
            Console.ReadLine();
        }
    }
}
