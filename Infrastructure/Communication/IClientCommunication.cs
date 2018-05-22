using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Communication;


namespace Communication
{
    public interface IClientCommunication
    {
        void Close();
        void Start();
        int Send(string msg);
    }
}
