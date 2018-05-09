using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication.Events;

namespace Communication
{
    public class ClientHandler : IClientHandler
    {
        private TcpClient m_client;
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryReader m_writer;
        private CancellationTokenSource m_cancelToken;

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public ClientHandler(TcpClient client)
        {
            m_client = client;
            m_stream = client.GetStream();
            m_reader = new BinaryReader(m_stream, Encoding.ASCII);
            m_writer = new BinaryReader(m_stream, Encoding.ASCII);
            m_cancelToken = new CancellationTokenSource();
        }

        public void Start()
        {
            string msg;
            new Task(() =>
            {
                while(true)
                {
                    try
                    {
                        msg = m_reader.ReadString();
                        if (msg != null)
                        {
                            DataReceived?.Invoke(this, new DataReceivedEventArgs(msg));
                        }
                    }
                    catch (Exception e)
                    {
                        DataReceived?.Invoke(this, new DataReceivedEventArgs("Client channel was closed"));
                        Close();
                    }
                }
            }, m_cancelToken.Token).Start();
        }

        public void Close()
        {
            m_cancelToken.Cancel();
        }
    }
}
