using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.Events;
using ImageService.Infrastructure.Enums;

namespace UI.Model
{
    public class ModelCommunicationHandler 
    {
        private TCPServiceClient client;
        private static ModelCommunicationHandler instance;

        public static ModelCommunicationHandler Instance
        {
            get
            {
                return instance ?? (instance = new ModelCommunicationHandler());
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
            client = new TCPServiceClient();
            client.DataReceived += OnDataReceived;
        }
    }
}
