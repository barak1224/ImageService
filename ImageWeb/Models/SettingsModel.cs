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
        private ConfigInfo m_confInfo;

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
            WasRequested = false;
            m_communication = ModelCommunicationHandler.Instance;
            if (m_communication.IsConnected)
            {
                m_confInfo = ConfigInfo.Instance;
            }
        }

        public void SettingsRequest()
        {
            m_confInfo.SendConfigRequest();
            SourceName = m_confInfo.SourceName;
            LogName = m_confInfo.LogName;
            OutputDirName = m_confInfo.OutputDir;
            ThumbnailSize = m_confInfo.ThumbnailSize;
            Directories = m_confInfo.Directories;
            WasRequested = true;
        }
    }
}