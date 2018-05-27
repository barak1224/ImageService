using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using Infrastructure.Events;
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
        private Dictionary<int, CommandExecute> m_commands;
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

        /// <summary>
        /// C'tor
        /// </summary>
        public LogModel()
        {
            m_logEntries = new ObservableCollection<MessageRecievedEventArgs>();
            m_commands = new Dictionary<int, CommandExecute>
            {
                {(int)CommandEnum.LogCommand, SetLogEntries }
            };
            communicationHandler = ModelCommunicationHandler.Instance;
            communicationHandler.DataReceived += GetCommand;
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.LogCommand;
            mc.CommandMsg = "";
            communicationHandler.Client.Send(mc.ToJSON());
            Thread.Sleep(100);
        }

        /// <summary>
        /// Sets the log entries.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void SetLogEntries(string msg) {
            List<string> logsList = JsonConvert.DeserializeObject<List<string>>(msg);
            foreach (string log in logsList)
            {
                m_logEntries.Insert(0,MessageRecievedEventArgs.ParseFromString(log));
            }
        }

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        private void GetCommand(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageCommand mc = MessageCommand.FromJSON(e.Message);
                if (m_commands.ContainsKey(mc.CommandID))
                {
                    m_commands[mc.CommandID](mc.CommandMsg);
                }
        }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="name">The name.</param>
        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
