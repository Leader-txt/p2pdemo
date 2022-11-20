using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace p2pdemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new UdpClient(new IPEndPoint(IPAddress.Any,333));
            var clients = new IPEndPoint[2];
            while (true)
            {
                if(clients[0] == null)
                {
                    server.Receive(ref clients[0]);
                    Console.WriteLine($"client0:{clients[0]}");
                }
                else
                {
                    server.Receive(ref clients[1]);
                    Console.WriteLine($"client1:{clients[1]}");
                    var data0 = Encoding.UTF8.GetBytes(clients[1].ToString()+":0");
                    var data1 = Encoding.UTF8.GetBytes(clients[0].ToString()+":1");
                    server.Send(data0, data0.Length, clients[0]);
                    server.Send(data1, data1.Length, clients[1]);
                    clients[0]=clients[1]=null;
                }
            }
        }
    }
}
