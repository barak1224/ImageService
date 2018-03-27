
using ImageService.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /**
         * The function raising a event to send the message to the logger
         */
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs messageArgs = new MessageRecievedEventArgs();
            messageArgs.Status = type;
            messageArgs.Message = message;
            MessageRecieved?.Invoke(this, messageArgs);
        }
    }
}
