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

            if (DashboardView.Instance != null)
            {
                mainFrame?.Navigate(DashboardView.Instance);
            }
            else
            {
                mainFrame?.Navigate(new DashboardView());
            }
        }

        private void ShowAllCommissionsPage(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainFrame = mainWindow.MainFrame;

            if (!(mainFrame.Content is CommissionBookView))
            {
                mainFrame.Navigate(new CommissionBookView());
            }
          
        }

        private void ShowClientBookView(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            mainFrame = mainWindow.MainFrame;

            if (!(mainFrame.Content is ClientBookView))
            {
                mainFrame.Navigate(new ClientBookView());
            }

        }
    }
}
