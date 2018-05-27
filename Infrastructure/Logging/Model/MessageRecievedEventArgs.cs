using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Model
{
    /// <summary>
    /// The class that holds the details for messaging for the logging
    /// </summary>
    public class MessageRecievedEventArgs : EventArgs
    {
        public MessageTypeEnum Status { get; set; }
        public string Message { get; set; }

        public string StatusAsString
        {
            get
            {
                return (Status as Enum).ToString();
            }
        }

        /// <summary>
        /// C'tor.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="message">The message.</param>
        public MessageRecievedEventArgs(MessageTypeEnum status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// Parses from string.
        /// </summary>
        /// <param name="messageReceived">The message received.</param>
        /// <returns></returns>
        public static MessageRecievedEventArgs ParseFromString(string messageReceived)
        {
            string[] args = messageReceived.Split(';');
            MessageTypeEnum type = MessageTypeEnumParser.ParseTypeFromString(args[0]);
            string message = args[1];
            return new MessageRecievedEventArgs(type, message);
        }
    }
}
