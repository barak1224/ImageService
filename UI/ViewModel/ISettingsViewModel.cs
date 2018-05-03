using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace UI
{
    internal interface ISettingsViewModel : INotifyPropertyChanged
    {
        ICommand SubmitRemove { get; }
        string OutputDirName { get; }
        string SourceName { get; }
        string LogName { get; }
        int ThumbnailSize { get; }
        string SelectedDir { get; }
        ObservableCollection<string> Directories { get; }
    }
}