using ImageService.Infrastructure.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Communication
{
    public class MessageCommand
    {
        public int CommandID { get; set; }
        public string CommandMsg { get; set; }

        public string ToJSON()
        {
            JObject job = new JObject();
            job["CommandID"] = CommandID;
            job["CommandMsg"] = CommandMsg;
            return job.ToString();
        }

        public static MessageCommand FromJSON(string msg)
        {
            JObject job = JObject.Parse(msg);
            MessageCommand cm = new MessageCommand();
            cm.CommandID = (int)job["CommandID"];
            cm.CommandMsg = (string)job["CommandMsg"];
            return cm;
        }
    }
}
