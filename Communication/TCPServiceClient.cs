using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Communication.Events;

namespace Communication
{
    public class TCPServiceClient : IClientCommunication
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        private TcpClient Client { get; set; }
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;

        public TCPServiceClient()
        {
            Client = new TcpClient();
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            Client.Connect(ep);
            m_stream = Client.GetStream();
            m_reader = new BinaryReader(m_stream, Encoding.ASCII);
            m_writer = new BinaryWriter(m_stream, Encoding.ASCII);
            Start();

        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Send(string msg)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            new Task(() =>
            {
                while(true)
                {
                    try
                    {
                        string message = m_reader.ReadString();
                        DataReceived?.Invoke(this, new DataReceivedEventArgs(message));
                    }
                    catch(SocketException)
                    {
                        break;
                    }
                }

            });
        }
    }
}
