using ImageService.Infrastructure.Enums;
using Infrastructure.Logging.Model;
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
        delegate void CommandExecute(string msg);
        private Dictionary<CommandEnum, CommandExecute> m_commands;
        public ObservableCollection<MessageRecievedEventArgs> LogEntries { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
