using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    class LogModel : ILogModel
    {
        public LogEntryList LogList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ModelCommunicationHandler communicationHandler;

        public LogModel()
        {
            LogList = new LogEntryList();
        }

        public void GetLogList()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("GetLogList");
                reader.Read();
            }
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
