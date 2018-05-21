using Infrastructure.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class DataReceivedEventArgs : EventArgs
    {
        public DataReceivedEventArgs(MessageCommand msg)
        {
            Message = msg;
        }
        public MessageCommand Message { get; set; }
    }
}
