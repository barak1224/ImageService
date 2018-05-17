using ImageService.Infrastructure.Enums;
using Infrastructure.Logging.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace UI.Model
{
    class LogModel : ILogModel
    {
        delegate void CommandExecute(string msg);
        private Dictionary<CommandEnum, CommandExecute> m_commands;
        public ModelCommunicationHandler communicationHandler;

        private ObservableCollection<MessageRecievedEventArgs> m_logEntries;

        public ObservableCollection<MessageRecievedEventArgs> LogEntries
        {
            get
            {
                return m_logEntries;
            }
            set
            {
                m_logEntries = value;
                NotifyPropertyChanged("LogEntries");
            }
        }

        public LogModel()
        {
            m_logEntries = new ObservableCollection<MessageRecievedEventArgs>();
            m_commands = new Dictionary<CommandEnum, CommandExecute>
            {
                {CommandEnum.LogCommand, SetLogEntries }
            };
            communicationHandler = ModelCommunicationHandler.Instance;
            communicationHandler.DataReceived += GetCommand;
            string message = JsonConvert.SerializeObject(CommandEnum.LogCommand) + ";";
            communicationHandler.Client.Send(message);
            Thread.Sleep(100);
            communicationHandler.Client.Start();
        }

        private void SetLogEntries(string msg)
        {
            JObject jsonData = JObject.Parse(msg);
            string logs = (string)jsonData;
            LogEntries = GetLogFromStringList(JsonConvert.DeserializeObject<ObservableCollection<string>>(logs));
            
        }

        private ObservableCollection<MessageRecievedEventArgs> GetLogFromStringList(ObservableCollection<string> logsList)
        {
            ObservableCollection<MessageRecievedEventArgs> logEntries = new ObservableCollection<MessageRecievedEventArgs>();
            foreach (string log in logsList)
            {
                logEntries.Add(MessageRecievedEventArgs.ParseFromString(log));
            }
            return logEntries;
        }

        private void GetCommand(object sender, ModelCommandArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (m_commands.ContainsKey(e.Command))
                {
                    m_commands[e.Command](e.Message);
                }
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
