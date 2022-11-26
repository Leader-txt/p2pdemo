using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace p2pdemo_client
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
            var server = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 333);
            if (!File.Exists("ip.txt"))
            {
                File.WriteAllText("ip.txt", "127.0.0.1:333");
                Console.WriteLine("请修改ip.txt为您的服务器地址");
                Console.WriteLine("按下回车键退出....");
                Console.ReadLine();
                Environment.Exit(0);
            }
            else
            {
                var addr = File.ReadAllText("ip.txt").Split(':');
                server = new IPEndPoint(IPAddress.Parse(addr[0]), int.Parse(addr[1]));
            }
            var udp=new UdpClient();
            udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            udp.Send(new byte[0], 0, server);
            var temp = new IPEndPoint(IPAddress.Any, 0);
            var buffer = Encoding.UTF8.GetString(udp.Receive(ref temp)).Split(':');
            temp = null;
            Console.WriteLine(string.Join(":", buffer));
            var oth = new IPEndPoint(IPAddress.Parse(buffer[0]), int.Parse(buffer[1]));
            var data = Encoding.UTF8.GetBytes("hello");
            udp.Send(data,data.Length, oth);
            new Thread(() =>
            {
                while (true)
                {
                    data=Encoding.UTF8.GetBytes("heart"+DateTime.Now.ToString());
                    udp.Send(data,data.Length, temp==null?oth:temp);
                    Thread.Sleep(1000);
                }
            })
            { IsBackground = true }.Start();
            new Thread(() =>
            {
                while (true)
                {
                    var info = Encoding.UTF8.GetString(udp.Receive(ref temp));
                    if (!info.StartsWith("heart"))
                    {
                        Console.WriteLine(info);
                    }
                }
            })
            { IsBackground = true }.Start();
            /*var tcp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcp.Bind(udp.Client.LocalEndPoint);
            tcp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            tcp.Connect(oth);*/
            while (true)
            {
                data=Encoding.UTF8.GetBytes(Console.ReadLine());
                udp.Send(data,data.Length, temp==null?oth:temp);
            }
            Console.ReadLine();
        }
    }
}
