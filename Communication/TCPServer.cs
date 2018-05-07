using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    class TCPServer
    {
        List<TcpClient> clients = new List<TcpClient>();
        private int port;
        private TcpListener listener;
        private IClientHandler ch;

        public TCPServer(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            listener = new TcpListener(ep);

            listener.Start();
            Console.WriteLine("Waiting for conections...");

            Task task = new Task(() =>
            {
                IClientHandler ch;
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        clients.Add(client);
                        ch = new ClientHandler(client);
                        ch.Start();
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
