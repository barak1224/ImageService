using Communication;
using Infrastructure.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Model;

namespace UI.ViewModel
{
    class MainWindowViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool m_isConnected;
        private ModelCommunicationHandler m_communicationClient;

        public bool IsConnected
        {
            get
            {
                return m_isConnected;
            }
            set
            {
                m_isConnected = value;
                NotifyPropertyChanged("IsConnected");
            }
        }

        /// <summary>
        /// C'tor
        /// </summary>
        public MainWindowViewModel()
        {
            m_communicationClient = ModelCommunicationHandler.Instance;
            IsConnected = m_communicationClient.IsConnected;
        }

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        /// <param name="name">The name.</param>
        private void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
