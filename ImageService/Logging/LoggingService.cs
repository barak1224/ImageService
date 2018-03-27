
using Service.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs messageArgs = new MessageRecievedEventArgs();
            messageArgs.Status = type;
            messageArgs.Message = message;
            MessageRecieved?.Invoke(this, messageArgs);
        }
    }
}
