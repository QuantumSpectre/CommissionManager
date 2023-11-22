using CommissionManager.GUI.Models;
using CommissionManager.GUI.UserControls;
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
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : Page
    {
        public UserProfile Profile { get; set; }
        public Frame? MainFrame { get; set; }
        

        public DashboardView()
        {
            InitializeComponent();
        }

        public void Initialize(UserProfile profile, Frame mainFrame)
        {
            Profile = profile;
            MainFrame = mainFrame;
            Welcome_Text.Text = "Welcome " + profile.Username;


        }

        private void ShowSettingsPage(object sender, RoutedEventArgs e)
        {
            if (MainFrame != null)
            {
                MainFrame.Navigate(new SettingsView());
            }
        }

        private void NavigateToCommissionView(Commission commission)
        {
            if (MainFrame != null)
            {
                CommissionView commissionView = new CommissionView();
                commissionView.DataContext = commission;
                MainFrame.Navigate(commissionView);
            }
        }

        private void CommissionPreview_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is CommissionPreview commissionPreview)
            {
                Commission commissionData = (Commission)commissionPreview.DataContext;
                NavigateToCommissionView(commissionData);
            }
        }
    }
}
