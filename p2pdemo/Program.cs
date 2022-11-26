using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace p2pdemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (o, e) =>
            {
                Console.WriteLine(e.ExceptionObject);
                Console.ReadLine();
            };
            Console.WriteLine("请输入监听端口");
            var server = new UdpClient(int.Parse(Console.ReadLine()));
            Console.WriteLine("服务器已启动");
            while (true)
            {
                var user0=new IPEndPoint(IPAddress.Any, 0);
                var user1=new IPEndPoint(IPAddress.Any, 0);
                server.Receive(ref user0);
                Console.WriteLine(user0);
                server.Receive(ref user1);
                Console.WriteLine(user1);
                var data = Encoding.UTF8.GetBytes(user1.ToString());
                server.Send(data, data.Length, user0);
                data = Encoding.UTF8.GetBytes(user0.ToString());
                server.Send(data, data.Length, user1);
            }
        }
    }
}
