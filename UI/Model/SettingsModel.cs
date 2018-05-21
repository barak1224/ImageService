using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using Infrastructure.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace UI.Model
{
    class SettingsModel : ISettingsModel
    {
        delegate void CommandExecute(string msg);
        private Dictionary<int, CommandExecute> m_commands;
        public ModelCommunicationHandler communicationHandler;

        private string m_outputDirName;
        private string m_sourceName;
        private string m_logName;
        private ObservableCollection<string> m_directories;
        private int m_thumbnailSize;


        public ObservableCollection<string> Directories
        {
            get
            {
                return this.m_directories;
            }
            set
            {
                this.m_directories = value;
                NotifyPropertyChanged("Directories");
            }
        }

        public SettingsModel()
       
        {
            // need to remove this
            m_commands = new Dictionary<int, CommandExecute>
            {
                {(int)CommandEnum.GetConfigCommand, SetConfigSettings },
                {(int)CommandEnum.CloseCommand, RemoveDir }
            };
            communicationHandler = ModelCommunicationHandler.Instance;
            communicationHandler.DataReceived += GetCommand;
            Thread.Sleep(1000);
            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.GetConfigCommand;
            mc.CommandMsg = "";
            communicationHandler.Client.Send(mc.ToJSON());
        }

        private void RemoveDir(string msg)
        {
            ;
        }

        private void SetConfigSettings(string msg)
        {
            JObject jsonAppConfig = JObject.Parse(msg);
            SourceName = (string)jsonAppConfig["Source Name"];
            LogName = (string)jsonAppConfig["Log Name"];
            OutputDirName = (string)jsonAppConfig["OutputDir"];
            ThumbnailSize = (int)jsonAppConfig["Thumbnail Size"];
            string dirs = (string)jsonAppConfig["Directories"];
            Directories = JsonConvert.DeserializeObject<ObservableCollection<string>>(dirs);
        }

        private void GetCommand(object sender, DataReceivedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageCommand mc = e.Message;
                if (m_commands.ContainsKey(mc.CommandID))
                {
                    m_commands[mc.CommandID](mc.CommandMsg);
                }
            }));
        }

        public string OutputDirName
        {
            get
            {
                return m_outputDirName;
            }
            set
            {
                m_outputDirName = value;
                NotifyPropertyChanged("OutputDirName");
            }
        }
        public string SourceName
        {
            get
            {
                return m_sourceName;
            }
            set
            {
                m_sourceName = value;
                NotifyPropertyChanged("SourceName");
            }
        }
        public string LogName
        {
            get
            {
                return m_logName;
            }
            set
            {
                m_logName = value;
                NotifyPropertyChanged("LogName");
            }
        }

        public int ThumbnailSize
        {
            get
            {
                return m_thumbnailSize;
            }
            set
            {
                m_thumbnailSize = value;
                NotifyPropertyChanged("ThumbnailSize");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SendRemoveDir(string dir)
        {
            String message = JsonConvert.SerializeObject(CommandEnum.CloseCommand) + ";" + dir;
            communicationHandler.Client.Send(message);
        }
    }
}