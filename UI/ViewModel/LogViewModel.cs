using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModel
{
    public class LogViewModel
    {
        public LogEntryList LogEntries { get; set; }

        public LogViewModel()
        {
            LogEntries = new LogEntryList { new LogEntry(LType.INFO, "This in info"),
                new LogEntry(LType.ERROR, "This in error"),
                new LogEntry(LType.WARNING, "This in warning") };
        }
    }

    public class LogEntryList : ObservableCollection<LogEntry> { }

    public class LogEntry
    {
        public LogEntry(LType logType, string message)
        {
            LogType = logType;
            Message = message;
        }

        public LType LogType { get; }
        public String Message { get; }

        public String GetLogTypeAsString(LType type)
        {
            if (type == LType.INFO)
                return "INFO";
            else if (type == LType.ERROR)
                return "ERROR";
            else return "WARNING";
        }
    }

    public enum LType
    {
        INFO,
        ERROR,
        WARNING,
    }
}
