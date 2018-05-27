using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Events;
using Infrastructure.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;

namespace Infrastructure.Communication
{
    public class ClientHandler : IClientHandler
    {
        private TcpClient m_client;
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        private List<int> m_commands;
        private CancellationTokenSource m_cancelToken;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public ClientHandler(TcpClient client)
        {
            m_client = client;
            m_stream = client.GetStream();
            m_reader = new BinaryReader(m_stream);
            m_writer = new BinaryWriter(m_stream);
            m_cancelToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            string msg;
            new Task(() =>
            {
                while (true)
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
                        MessageCommand mc = new MessageCommand();
                        mc.CommandID = -1;
                        mc.CommandMsg = e.Message;
                        DataReceived?.Invoke(this, new DataReceivedEventArgs(mc.ToJSON()));
                        Close();
                        break;
                    }
                }
            }, m_cancelToken.Token).Start();
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            m_writer.Close();
            m_reader.Close();
            m_stream.Close();
            m_cancelToken.Cancel();
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public int Send(string message)
        {
            lock (m_writer)
            {
                try
                {
                    m_writer.Write(message);
                    return 1;
                } catch (Exception e)
                {
                    return 0;
                }
            }
        }
    }
}