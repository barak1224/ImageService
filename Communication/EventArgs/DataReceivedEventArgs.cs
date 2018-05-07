using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.EventArgs
{
    class DataReceivedEventArgs
    {
        public DataReceivedEventArgs(string msg)
        {
            Message = msg;
        }
        public string Message { get; set; }
    }
}
