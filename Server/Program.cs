using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        public static Socket[] client;
        public static void Main(string[] args)
        {
            byte[] send = new byte[1024];
            byte[] receive = new byte[1024];       

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipep = new IPEndPoint(ip, 8888);
            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(ipep);
            server.Listen(10);

            Console.WriteLine("Server is waiting for Client to join...");
            client = new Socket[100];
            client[0] = server.Accept();        

            while (true)
            {
                int ive = client[0].Receive(receive);
                string s = Encoding.ASCII.GetString(receive, 0, ive);
                int m = int.Parse(s);
                Console.WriteLine(m);

                int kq;
                if (m < 5)
                {
                    kq = m + 1;
                    string e = kq.ToString();
                    send = Encoding.ASCII.GetBytes(e);
                    client[0].Send(send, send.Length, SocketFlags.None);
                }
                else
                {
                    string tb = "Play Again";
                    send = Encoding.ASCII.GetBytes(tb);
                    client[0].Send(send, send.Length, SocketFlags.None);
                }

            }
        }
    }
}
