using Communication;
using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using Infrastructure.Events;
using Infrastructure.Logging.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageWeb.Models
{
    public class LogModel
    {
        private ModelCommunicationHandler m_communication;
        private List<MessageRecievedEventArgs> m_logEntries;
        public List<MessageRecievedEventArgs> LogEntries
        {
            get => m_logEntries; private set => m_logEntries = value;
        }

        public LogModel()
        {
            m_communication = ModelCommunicationHandler.Instance;
            if (m_communication.IsConnected)
            {
                m_communication.DataReceived += GetCommand;
                LogEntries = new List<MessageRecievedEventArgs>();
            }
        }

        public void LogsRequest()
        {
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.LogCommand;
            mc.CommandMsg = "";
            string m = mc.ToJSON();
            m_communication.Client.Send(m);
            Thread.Sleep(100);
        }

        private void GetCommand(object sender, DataReceivedEventArgs e)
        {
            MessageCommand mc = MessageCommand.FromJSON(e.Message);
            if (mc.CommandID == (int)CommandEnum.LogCommand)
            {
                LogsRecived(mc.CommandMsg);
            }
        }

        private void LogsRecived(string msg)
        {
            m_logEntries.Clear();
            List<string> logsList = JsonConvert.DeserializeObject<List<string>>(msg);
            foreach (string log in logsList)
            {
                m_logEntries.Insert(0, MessageRecievedEventArgs.ParseFromString(log));
            }
        }
    }
}