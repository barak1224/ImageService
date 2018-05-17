using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Model
{
    /// <summary>
    /// The enum to the message type enum
    /// </summary>
    public enum MessageTypeEnum : int
    {
        INFO,
        WARNING,
        FAIL
    }

    public static class MessageTypeEnumParser
    {
        public static MessageTypeEnum ParseTypeFromString(string type)
        {
            switch(type)
            {
                case "INFO": return MessageTypeEnum.INFO;
                case "FAIL": return MessageTypeEnum.FAIL;
                case "WARNING": return MessageTypeEnum.WARNING;
                default: return 0;
            }
        }
    }
}
