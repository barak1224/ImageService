using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    class SettingsModel : ISettingsModel
    {
        delegate void CommandExecute(string msg);
        private Dictionary<CommandEnum, CommandExecute> m_commands;
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
            this.OutputDirName = "Output Directory";
            this.SourceName = "Source Name";
            this.LogName = "Log Name";
            this.ThumbnailSize = 120;
            m_directories = new ObservableCollection<string> { "iosi", "is", "gay" };
            m_commands = new Dictionary<CommandEnum, CommandExecute>
            {
                {CommandEnum.GetConfigCommand, setConfigSettings }
            };
            communicationHandler = ModelCommunicationHandler.Instance;
            communicationHandler.DataReceived += GetCommand;
            communicationHandler.Client.Start();
            string message = JsonConvert.SerializeObject(CommandEnum.GetConfigCommand) + ";";
            communicationHandler.Client.Send(message);
        }

        private void setConfigSettings(string msg)
        {
            JObject jsonAppConfig = new JObject(msg);
            SourceName = (string)jsonAppConfig["Source Name"];
            LogName = (string)jsonAppConfig["Log Name"];
            OutputDirName = (string)jsonAppConfig["OutputDir"];
            string dirs = (string)jsonAppConfig["Directories"];
            Directories = JsonConvert.DeserializeObject<ObservableCollection<string>>(dirs);
        }

        private void GetCommand(object sender, ModelCommandArgs e)
        {
            if (m_commands.ContainsKey(e.Command))
            {
                m_commands[e.Command](e.Message);
            }
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

        public void RemoveDir(string dir)
        {

        }
    }
}