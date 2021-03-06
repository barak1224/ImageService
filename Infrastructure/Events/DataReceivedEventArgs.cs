﻿using Infrastructure.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// C'tor
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public DataReceivedEventArgs(string msg)
        {
            Message = msg;
        }
        public string Message { get; set; }
    }
}
