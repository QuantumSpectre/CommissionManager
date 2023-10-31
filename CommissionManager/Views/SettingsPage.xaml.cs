using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using CommissionManager;

namespace CommissionManager
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
       public Frame? mainFrame {  get; set; }

        public SettingsPage()
        {
            InitializeComponent();

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;
        }

        private void ReturnToDashboard(object sender, RoutedEventArgs e)
        {
            if ( mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
            }
        }
    }
}
