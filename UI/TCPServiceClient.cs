using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Events;

namespace UI
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
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8001);
            Client.Connect(ep);
            m_stream = Client.GetStream();
            m_reader = new BinaryReader(m_stream, Encoding.ASCII);
            m_writer = new BinaryWriter(m_stream, Encoding.ASCII);
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Send(string msg)
        {
            try
            {
                m_writer.Write(msg);
                return 1;
            } catch (Exception e) {
                return 0;
            }
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
                        if (message != null)
                        {
                            DataReceived?.Invoke(this, new DataReceivedEventArgs(message));
                        }
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
