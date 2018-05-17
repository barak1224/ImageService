using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Model;

namespace UI
{
    public class LogViewModel : ILogViewModel
    {
        private ILogModel LogModel;

        //public LogEntryList LogList { get; }

        public LogViewModel()
        {
            LogModel = new LogModel();

            //    // just for the test, to be removed
            //    LogList = new LogEntryList { new LogEntry(LType.INFO, "This in info, IMA"),
            //        new LogEntry(LType.ERROR, "This in error, SHEL"),
            //        new LogEntry(LType.WARNING, "This in warning, BARAK") };
            //}

            //public event PropertyChangedEventHandler PropertyChanged;

            //public void NotifyPropertyChanged(string name)
            //{
            //    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //}

            //internal class LogAsString
            //{
            //    public string Type { get; set; }
            //    public string Color { get; set; }
            //    public string Message { get; set; }
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
