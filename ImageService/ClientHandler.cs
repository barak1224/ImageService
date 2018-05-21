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
using ImageService.Controller;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;

namespace ImageService
{
    public class ClientHandler : IClientHandler
    {
        private TcpClient m_client;
        private NetworkStream m_stream;
        private BinaryReader m_reader;
        private BinaryWriter m_writer;
        private List<int> m_commands;
        private CancellationTokenSource m_cancelToken;
        private IImageController m_controller;
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public ClientHandler(TcpClient client, ref IImageController controller)
        {
            m_client = client;
            m_stream = client.GetStream();
            m_reader = new BinaryReader(m_stream);
            m_writer = new BinaryWriter(m_stream);
            m_cancelToken = new CancellationTokenSource();
            m_controller = controller;
            m_commands = new List<int>()
            {
                ((int)CommandEnum.GetConfigCommand),
                ((int)CommandEnum.LogCommand)
            };
        }

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
                            MessageCommand mc = MessageCommand.FromJSON(msg);
                            if (m_commands.Contains(mc.CommandID))
                            {
                                string convert = m_controller.ExecuteCommand(mc.CommandID, null, out bool result);
                                mc.CommandMsg = convert;
                                m_writer.Write(mc.ToJSON());
                            }
                            else
                            {
                                DataReceived?.Invoke(this, new DataReceivedEventArgs(mc));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageCommand mc = new MessageCommand();
                        mc.CommandID = -1;
                        mc.CommandMsg = e.Message;
                        DataReceived?.Invoke(this, new DataReceivedEventArgs(mc));
                        Close();
                        break;
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