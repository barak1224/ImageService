using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class TCPServer
    {
        private int port;
        private TcpListener listener;
        private IClientHandler ch;

        public TCPServer(int port, IClientHandler ch)
        {
            this.port = port;
            this.ch = ch;
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine("Waiting for conections...");

            Task task = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine();
                        ch.HandleClient(client);
                    }
                    catch(SocketException)
                    {
                        break;
                    }
                }
            });
            task.Start();
        }
    }
}
