using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Communication;


namespace UI
{
    public interface IClientCommunication: IClientHandler
    {
        int Send(string msg);
    }
}
