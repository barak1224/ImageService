using Communication;
using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using Infrastructure.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace ImageWeb.Models
{
    public class ConfigInfo
    {
        private static ConfigInfo m_instance;
        private ModelCommunicationHandler m_communication;
        private object theLock;
        private bool Updated { get; set; }
        public string[] m_validExtensions = { "*.jpg", "*.png", "*.gif", "*.bmp" };
        public string[] ValidExtensions { get => m_validExtensions; }

        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Directories")]
        public List<string> Directories { get; set; }

        #endregion

        #region Instance
        public static ConfigInfo Instance {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new ConfigInfo();
                }
                return m_instance;
            }
            set
            {
                m_instance = value;
            }
        }

        private ConfigInfo()
        {
            m_communication = ModelCommunicationHandler.Instance;
            m_communication.Client.DataReceived += GetCommand;
            theLock = new object();
            Updated = false;
        }
        #endregion

        public void SendConfigRequest()
        {

            MessageCommand mc = new MessageCommand();
            mc.CommandID = (int)CommandEnum.GetConfigCommand;
            mc.CommandMsg = "";
            string m = mc.ToJSON();
            m_communication.Client.Send(m);
            lock(theLock)
            {
                Monitor.Wait(theLock);
            }
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
            lock (theLock)
            {
                JObject jsonAppConfig = JObject.Parse(msg);
                SourceName = (string)jsonAppConfig["Source Name"];
                LogName = (string)jsonAppConfig["Log Name"];
                OutputDir = (string)jsonAppConfig["OutputDir"];
                ThumbnailSize = (int)jsonAppConfig["Thumbnail Size"];
                string dirs = (string)jsonAppConfig["Directories"];
                Directories = JsonConvert.DeserializeObject<List<string>>(dirs);
                Monitor.Pulse(theLock);
            }
        }
    }
}