using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using Infrastructure.Events;
using Infrastructure.Logging.Model;

namespace ImageService
{
    public class TCPServiceServer
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        List<IClientHandler> clients = new List<IClientHandler>();
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
                        ch = new ClientHandler(client, ref m_controller);
                        clients.Add(ch);
                        ch.DataReceived += MoveToServer;
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

        private void MoveToServer(object sender, DataReceivedEventArgs e)
        {
            if (e.Message.CommandID == -1)
            {
                IClientHandler c = sender as IClientHandler;
                clients.Remove(c);
            }
            else
            {
                DataReceived?.Invoke(sender, e);
            }
        }

        public void SendAll(string messageCommand)
        {
            foreach (IClientHandler client in clients) {
                if (client.Send(messageCommand) == 0)
                {
                    client.Close();
                    clients.Remove(client);
                }
            }
        }

        internal void Close()
        {
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.CloseServerCommand;
            SendAll(mc.ToJSON());
        }
    }
}
