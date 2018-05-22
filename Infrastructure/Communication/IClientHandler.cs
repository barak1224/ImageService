using System;
using System.Collections.Generic;
using Infrastructure.Events;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Communication
{
    public interface IClientHandler
    {
        event EventHandler<DataReceivedEventArgs> DataReceived;
        void Start();
        void Close();
        int Send(string message);
    }
}
