using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;
using Infrastructure.Communication;

namespace ImageService
{
    public class TCPServiceServer
    {
        List<TcpClient> clients = new List<TcpClient>();
        private int m_port;
        private TcpListener listener;
        private IClientHandler ch;
        private IImageController m_controller;

        public TCPServiceServer(int port, ref Controller.IImageController controller)
        {
            m_port = port;
            m_controller = controller;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), m_port);
            listener = new TcpListener(ep);
        }

        public void Start()
        {
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
                        ch = new ClientHandler(client, ref m_controller);
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
