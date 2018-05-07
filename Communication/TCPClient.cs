using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.EventArgs;

namespace Communication
{
    class TCPClient : IClientCommunication
    {
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Send(string msg)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
    }
}
