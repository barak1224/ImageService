using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;

namespace Communication
{
    public interface IClientCommunication: IClientHandler
    {
        int Send(string msg);
    }
}
