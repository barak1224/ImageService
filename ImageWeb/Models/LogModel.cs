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
        private object theLock = new object(); 
        private ModelCommunicationHandler m_communication;
        private List<MessageRecievedEventArgs> m_logEntries;
        public List<MessageRecievedEventArgs> LogEntries
        {
            get => m_logEntries; private set => m_logEntries = value;
        }
        public List<MessageRecievedEventArgs> OriginalLogEntries;

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
            lock(theLock) {
                Monitor.Wait(theLock);

            }
        }

        private void GetCommand(object sender, DataReceivedEventArgs e)
        {
            MessageCommand mc = MessageCommand.FromJSON(e.Message);
            if (mc.CommandID == (int)CommandEnum.LogCommand)
            {
                LogsRecieved(mc.CommandMsg);
            }
        }

        private void LogsRecieved(string msg)
        {
            LogEntries.Clear();
            List<string> logsList = JsonConvert.DeserializeObject<List<string>>(msg);
            foreach (string log in logsList)
            {
                m_logEntries.Insert(0, MessageRecievedEventArgs.ParseFromString(log));
            }
            OriginalLogEntries = new List<MessageRecievedEventArgs>(m_logEntries);
            lock(theLock)
            {
                Monitor.Pulse(theLock);
            }
        }

        public void filterLogsByType(string type)
        {
            MessageTypeEnum selectedType = MessageTypeEnumParser.ParseTypeFromString(type);
            LogEntries.Clear();
            foreach (MessageRecievedEventArgs log in OriginalLogEntries)
            {
                if (log.Status == selectedType)
                {
                    m_logEntries.Add(log);
                }
            }
        }
    }
}