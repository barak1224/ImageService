using Infrastructure.Logging.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Model
{
    interface ILogModel : INotifyPropertyChanged
    {
        ObservableCollection<MessageRecievedEventArgs> LogEntries { get; set; }
    }
}
