using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Events;
using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using System.Threading;
using Communication;
using System.Configuration;

namespace UI.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelCommunicationHandler
    {
        private static ModelCommunicationHandler instance;

        public TCPServiceClient Client { get; set; }
        public bool IsConnected { get; set; }
        public static ModelCommunicationHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ModelCommunicationHandler();
                    instance.Client.Start();
                }
                return instance;
            }
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        /// <summary>
        /// Called when [data received].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        public void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            MessageCommand mc = MessageCommand.FromJSON(e.Message);
            if (e != null)
            {
                if (mc.CommandID == (int)CommandEnum.CloseServerCommand)
                {
                    IsConnected = false;
                }
                else
                {
                    DataReceived?.Invoke(this, e);
                }
            }
        }

        /// <summary>
        /// C'tor.
        /// </summary>
        private ModelCommunicationHandler()
        {
            int port, ts;
            if (int.TryParse(ConfigurationManager.AppSettings["Port"], out ts))
            {
                port = ts;
            }
            else
            {
                port = 8000;
            }
            string ip = ConfigurationManager.AppSettings["IP"];
            Client = new TCPServiceClient(port, ip);
            Client.DataReceived += OnDataReceived;
            IsConnected = Client.IsConnected;
        }
    }
}
