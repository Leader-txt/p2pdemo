using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace p2pdemo_client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new UdpClient();
            var data = Encoding.UTF8.GetBytes("me");
            client.Send(data, data.Length, new IPEndPoint(IPAddress.Parse("49.232.218.188"), 333));

            var server = new IPEndPoint(IPAddress.Any, 0);
            var who = Encoding.UTF8.GetString(client.Receive(ref server));
            Console.WriteLine("who:"+who);
            var oth = new IPEndPoint(IPAddress.Parse(who.Split(':')[0]), int.Parse(who.Split(':')[1]));
            data = Encoding.UTF8.GetBytes("hello I'm client" + who.Split(':')[2]);
            client.Send(data, data.Length, oth);
            while (true)
            {
                var temp = new IPEndPoint(IPAddress.Any, 0);
                Console.WriteLine(who.Split(':')[2]+Encoding.UTF8.GetString(client.Receive(ref temp)));
                data = Encoding.UTF8.GetBytes(Guid.NewGuid() + "from client#" + who.Split(':')[2]+temp);
                client.Send(data,data.Length, oth);
            }
        }
    }
}
