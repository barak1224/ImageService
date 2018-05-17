using ImageService.Infrastructure.Enums;
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
using System.Windows.Media;

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
            communicationHandler = ModelCommunicationHandler.Instance;
            communicationHandler.DataReceived += GetCommand;
        }

        private void GetCommand(object sender, ModelCommandArgs e)
        {
            if (e.Command == CommandEnum.LogCommand)
            {
                // TODO
            }
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

        public SolidColorBrush Color { get; set; }

        private SolidColorBrush GetColorFromType(LType type)
        {
            switch (type)
            {
                case LType.INFO: return Brushes.Green;
                case LType.WARNING: return Brushes.Yellow ;
                case LType.ERROR: return Brushes.Red;
                default: return Brushes.White;
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
