using System;
using System.Windows;
using System.Windows.Controls;

namespace CommissionManager.UserControls
{
    /// <summary>
    /// Interaction logic for Sidebar.xaml
    /// </summary>
    public partial class Sidebar : UserControl
    {
        public Frame? mainFrame { get; set; }

        public Sidebar()
        {
            InitializeComponent();


        }

        private void ShowSettingsPage(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;


            if (!(mainFrame.Content is SettingsPage))
            {
                mainFrame.Navigate(new SettingsPage());
            }
            


        }

        private void ShowDashboardPage(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;


            if (!(mainFrame.Content is DashboardPage))
            {
                mainFrame.Navigate(new DashboardPage());
            }
        }
    }
}
