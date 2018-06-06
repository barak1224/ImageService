using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Communication;
using Infrastructure.Events;
using Infrastructure.Communication;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace ImageWeb.Models
{
    public class SettingsModel
    {
        private ModelCommunicationHandler m_communication;
        private int m_thumbnailSize;
        private string m_sourceName;
        private string m_logName;
        private string m_outputDir;
        private bool _wasRequested;

        #region Properties
        public bool WasRequested { get; set; }
        public string SourceName { get => m_sourceName ?? "N/A"; private set => m_sourceName = value; }
        public string LogName { get => m_logName ?? "N/A"; private set => m_logName = value; }
        public string OutputDirName { get => m_outputDir ?? "N/A"; private set => m_outputDir = value; }
        public int ThumbnailSize { get; set; }
        public List<string> Directories { get; private set; }
        #endregion

        public SettingsModel()
        {
            m_communication = ModelCommunicationHandler.Instance;
            if (m_communication.IsConnected)
            {
                m_communication.DataReceived += GetCommand;
            }
        }

        public void SendConfigRequest()
        {

            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.GetConfigCommand;
            mc.CommandMsg = "";
            string m = mc.ToJSON();
            m_communication.Client.Send(m);
            Thread.Sleep(1000);
        }

        private void GetCommand(object sender, DataReceivedEventArgs e)
        {
            MessageCommand mc = MessageCommand.FromJSON(e.Message);
            if (mc.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                SettingsRecived(mc.CommandMsg);
            }
        }
        private void SettingsRecived(string msg)
        {
            JObject jsonAppConfig = JObject.Parse(msg);
            SourceName = (string)jsonAppConfig["Source Name"];
            LogName = (string)jsonAppConfig["Log Name"];
            OutputDirName = (string)jsonAppConfig["OutputDir"];
            ThumbnailSize = (int)jsonAppConfig["Thumbnail Size"];
            string dirs = (string)jsonAppConfig["Directories"];
            Directories = JsonConvert.DeserializeObject<List<string>>(dirs);
        }
    }
}