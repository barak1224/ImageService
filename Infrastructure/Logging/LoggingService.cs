
using Infrastructure.Logging.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;

        /// <summary>
        /// The Function raising MessageRecieved event to notify about a 
        /// new message that need to be written to the event log.
        /// </summary>
        /// <param name="message"> The message to write the event log </param>
        /// <param name="type"> An enum type for the message </param>
        public void Log(string message, MessageTypeEnum type)
        {
            MessageRecievedEventArgs messageArgs = new MessageRecievedEventArgs();
            messageArgs.Status = type;
            messageArgs.Message = message;
            MessageRecieved?.Invoke(this, messageArgs);
        }
    }
}
