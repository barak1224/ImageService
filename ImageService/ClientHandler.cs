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
        private List<CommandEnum> m_commands;
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
            m_commands = new List<CommandEnum>()
            {
                CommandEnum.GetConfigCommand,
                CommandEnum.LogCommand
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
                            string[] args = msg.Split(';');
                            CommandEnum c = JsonConvert.DeserializeObject<CommandEnum>(args[0]);
                            if (m_commands.Contains(c))
                            {
                                string convert = m_controller.ExecuteCommand((int)c, null, out bool result);
                                string s = args[0] + ";" + convert;
                                m_writer.Write(s);
                            }
                            else if (c == CommandEnum.LogCommand)
                            {
                                string convert = m_controller.ExecuteCommand((int)CommandEnum.LogCommand, null, out bool result);
                                string s = "3;" + convert;
                                m_writer.Write(s);
                            }
                            else
                            {
                                DataReceived?.Invoke(this, new DataReceivedEventArgs(msg));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        DataReceived?.Invoke(this, new DataReceivedEventArgs("Client channel was closed"));
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