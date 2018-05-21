using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class LogCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            AppParsing app = AppParsing.Instance;
            EventLog log = new EventLog(app.LogName, ".");
            EventLogEntryCollection logEntries = log.Entries;
            List<string> logs = new List<string>();
            for (int index = 0; index < logEntries.Count; index++) {
                logs.Add(logEntries[index].Message.ToString());
            }
            string convertToString;
            if ((convertToString = JsonConvert.SerializeObject(logs)) == null)
            {
                result = false;
                return "Couldn't convert logs message into string";
            }
            result = true;
            return convertToString;
        }
    }
}