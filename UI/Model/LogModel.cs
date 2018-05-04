using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    class LogModel : ILogModel
    {
        public LogEntryList LogList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LogModel()
        {
            LogList = new LogEntryList();
        }
    }


    public class LogEntryList : ObservableCollection<LogEntry> { }

    public class LogEntry
    {
        public LogEntry(LType logType, string message)
        {
            LogType = logType;
            Message = message;
            TypeAsString = LogType.ToString();
            Color = GetColorFromType(LogType);
        }

        public LType LogType { get; }
        public string Message { get; }

        public string TypeAsString { get; set; }

        public string Color { get; set; }

        private string GetColorFromType(LType type)
        {
            switch (type)
            {
                case LType.INFO: return "green";
                case LType.WARNING: return "yellow";
                case LType.ERROR: return "red";
                default: return "white";
            }
        }
    }

    public enum LType
    {
        INFO,
        ERROR,
        WARNING,
    }
}
