﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Events;
using ImageService.Infrastructure.Enums;
using Infrastructure.Communication;
using System.Threading;
using Communication;

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

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e != null)
            {
                if (e.Message.CommandID == (int)CommandEnum.CloseServerCommand)
                {
                    IsConnected = false;
                }
                else
                {
                    DataReceived?.Invoke(this, e);
                }
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
