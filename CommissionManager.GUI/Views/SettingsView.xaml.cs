using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CommissionManager.GUI.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : Page
    {
             public Frame? mainFrame { get; set; }

        public SettingsView()
        {
            InitializeComponent();

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;
        }

        private void ReturnToDashboard(object sender, RoutedEventArgs e)
        {
            if (mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
            }
        }
    }
    
}
