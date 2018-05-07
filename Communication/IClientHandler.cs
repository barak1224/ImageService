﻿using System;
using System.Collections.Generic;
using Communication.EventArgs;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    interface IClientHandler
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        void Start();
        void Close();
    }
}
