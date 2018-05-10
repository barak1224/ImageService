using ImageService.Infrastructure.Enums;
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
        public ModelCommunicationHandler communicationHandler;

        private string m_outputDirName;
        private string m_sourceName;
        private string m_logName;

        private int m_thumbnailSize;

        private List<CommandEnum> commandList = new List<CommandEnum> {CommandEnum.NewFileCommand,
                                                 CommandEnum.GetConfigCommand,
                                                 CommandEnum.CloseCommand };

        public ObservableCollection<string> Directories
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public SettingsModel()
       
        {
            // need to remove this
            this.OutputDirName = "Output Directory";
            this.SourceName = "Source Name";
            this.LogName = "Log Name";
            this.ThumbnailSize = 120;

            communicationHandler = ModelCommunicationHandler.Instance;
            communicationHandler.DataReceived += GetCommand;
        }

        private void GetCommand(object sender, ModelCommandArgs e)
        {
            if (commandList.Contains(e.Command))
            {
                // TODO
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