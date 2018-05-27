using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Communication;
using Infrastructure.Events;

namespace Communication
{
    public class TCPServiceClient : IClientCommunication
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        private TcpClient Client { get; set; }

        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        public bool IsConnected { get; set; }

        /// <summary>
        /// C'tor
        /// </summary>
        public TCPServiceClient(int port, string ip)
        {
            try
            {
                Client = new TcpClient();
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
                Client.Connect(ep);
                m_stream = Client.GetStream();
                m_reader = new BinaryReader(m_stream);
                m_writer = new BinaryWriter(m_stream);
                IsConnected = true;
                Thread.Sleep(1000);

            } catch (Exception e)
            {
                IsConnected = false;
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Close()
        {
            m_stream.Close();
            m_reader.Close();
            m_writer.Close();
        }

        /// <summary>
        /// Sends the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Starts this instance.
        /// </summary>
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
                    catch(SocketException e)
                    {
                        IsConnected = false;
                        Close();
                    }
                }

            }).Start();
        }
    }
}
