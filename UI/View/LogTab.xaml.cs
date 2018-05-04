using System.Windows.Controls;

namespace UI.View
{
    /// <summary>
    /// Interaction logic for LogTab.xaml
    /// </summary>
    public partial class LogTab : UserControl
    {
        public LogTab()
        {
            InitializeComponent();
            DataContext = new LogViewModel();
        }
    }
}
