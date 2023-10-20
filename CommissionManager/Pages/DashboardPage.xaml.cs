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

namespace CommissionManager
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : UserControl
    {
        public Frame? mainFrame {  get; set; }

        public DashboardPage()
        {
            InitializeComponent();


            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;
        }

        private void ShowSettingsPage(object sender, RoutedEventArgs e)
        {
            if (mainFrame != null)
            {
                mainFrame.Navigate(new SettingsPage());
            }
        }
    }
}
