using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Logging.Model;
using UI.Model;

namespace UI
{
    public class LogViewModel : ILogViewModel
    {
        private ILogModel m_model;

        public ObservableCollection<MessageRecievedEventArgs> LogEntries
        {
            get
            {
                return m_model.LogEntries;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LogViewModel()
        {
            m_model = new LogModel();
            m_model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
