using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using UI.Model;

namespace UI
{
    class SettingsViewModel : ISettingsViewModel
    {
        private ISettingsModel m_model;
        public ICommand SubmitRemove { get; private set; }
        public string OutputDirName {
            get
            {
                return m_model.OutputDirName;
            }
        }
        public string SourceName
        {
            get
            {
                return m_model.SourceName;
            }
        }
        public string LogName
        {
            get
            {
                return m_model.LogName;
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return m_model.ThumbnailSize;
            }
        }
        private string m_selectedDir;
        public string SelectedDir
        {
            get
            {
                return m_selectedDir;
            }
            set
            {
                m_selectedDir = value;
                NotifyPropertyChanged("SelectedDir");
            }
        }

        /// <summary>
        /// Raising event when a proprty is changed
        /// </summary>
        /// <param name="name"></param>
        public void NotifyPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<string> Directories {
            get
            {
                return this.m_model.Directories;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// C'tor
        /// </summary>
        public SettingsViewModel()
        {
            m_model = new SettingsModel();
            m_model.PropertyChanged += this.PropertyChanged;
            this.SubmitRemove = new DelegateCommand<object>(this.OnSubmit, this.CanSubmit);
            this.PropertyChanged += RemoveCommand;

            // just for the test, to be removed
        }

        /// <summary>
        /// The function rasing the execute only when it can be execute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveCommand(object sender, PropertyChangedEventArgs e)
        {
            var command = SubmitRemove as DelegateCommand<object>;
            command.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// The function do the removing from the list after notifying the tcp client
        /// </summary>
        /// <param name="obj"></param>
        private void OnSubmit(object obj)
        {
            //this.m_model.Directories.Remove(SelectedDir);
            m_model.SendRemoveDir(SelectedDir);
            this.SelectedDir = null;
        }

        /// <summary>
        /// The fucntion checks if the button can be clicked
        /// </summary>
        /// <param name="obj"></param>
        /// <returns> true for selected item, otherwise false </returns>
        private bool CanSubmit(object obj)
        {
            return !(string.IsNullOrEmpty(this.SelectedDir));
        }
    }
}
