using CommissionManager.GUI.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CommissionManager.GUI.UserControls
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


            if (!(mainFrame.Content is SettingsView))
            {
                mainFrame.Navigate(new SettingsView());
            }
            


        }

        private void ShowDashboardPage(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;


            if (!(mainFrame.Content is DashboardView))
            {
                mainFrame.Navigate(new DashboardView());
            }
        }
    }
}
