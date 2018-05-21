using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Events;
using ImageService.Infrastructure.Enums;
using System.Threading;

namespace UI.Model
{
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

        public event EventHandler<ModelCommandArgs> DataReceived;

        public void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            ModelCommandArgs command = ParseData(e.Message);
            if (command != null)
            {
                DataReceived?.Invoke(this, command); 
            }
        }

        private ModelCommandArgs ParseData(string message)
        {
            int commandNum;
            string[] args = message.Split(';');
            if (int.TryParse(args[0], out commandNum))
            {
                return new ModelCommandArgs((CommandEnum)commandNum, args[1]);
            }
            else
            {
                return null;
            }            
        }

        private ModelCommunicationHandler()
        {
            Client = new TCPServiceClient();
            Client.DataReceived += OnDataReceived;
            IsConnected = Client.IsConnected;
        }
    }
}
