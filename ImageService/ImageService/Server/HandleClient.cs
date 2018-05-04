using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class HandleClient : IClientHandler
    {
        void IClientHandler.HandleClient(TcpClient client)
        {
            throw new NotImplementedException();
        }
    }
}
