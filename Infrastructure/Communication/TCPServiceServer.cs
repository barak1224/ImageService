using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using Infrastructure.Events;
using Infrastructure.Logging.Model;

namespace Infrastructure.Communication
{
    public class TCPServiceServer
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        List<IClientHandler> clients = new List<IClientHandler>();
        private int m_port;
        private TcpListener listener;
        private IClientHandler ch;

        /// <summary>
        /// C'tor.
        /// </summary>
        /// <param name="port">The port.</param>
        /// <param name="controller">The controller.</param>
        public TCPServiceServer(int port, string ip)
        {
            m_port = port;
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), m_port);
            listener = new TcpListener(ep);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
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
                        ch = new ClientHandler(client);
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

        /// <summary>
        /// Moves to server.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        private void MoveToServer(object sender, DataReceivedEventArgs e)
        {
            MessageCommand mc = MessageCommand.FromJSON(e.Message);
            if (mc.CommandID == -1)
            {
                IClientHandler c = sender as IClientHandler;
                clients.Remove(c);
            }
            else
            {
                DataReceived?.Invoke(sender, e);
            }
        }

        /// <summary>
        /// Sends all.
        /// </summary>
        /// <param name="messageCommand">The message command.</param>
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

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.CloseServerCommand;
            SendAll(mc.ToJSON());
            foreach (ClientHandler client in clients)
            {
                client.Close();
            }
        }
    }
}
