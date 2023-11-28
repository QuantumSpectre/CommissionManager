using CommissionManager.GUI.Models;
using CommissionManager.GUI.UserControls;
using Newtonsoft.Json;
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
        public HttpClientService httpClientService { get; set; }
        public UserProfile Profile { get; set; }
        public Frame? MainFrame { get; set; }
        public static DashboardView? Instance { get; set; }
        public MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public Commission CompletedCommission { get; set; }
        public Commission CurrentCommission { get; set; }
        public Commission QueuedCommission1 { get; set; }
        public Commission QueuedCommission2 { get; set; }

        public DashboardView()
        {
            InitializeComponent();

            CompletedCommission = new Commission();
            CurrentCommission = new Commission();
            QueuedCommission1 = new Commission();
            QueuedCommission2 = new Commission();
        }

        public async Task Initialize(UserProfile profile, Frame mainFrame)
        {
            Profile = profile;
            MainFrame = mainFrame;
            Welcome_Text.Text = "Welcome " + profile.Username;

            httpClientService = mainWindow._httpClientService;

            Instance = this;

            try
            {
                var httpResult = await httpClientService.GetAsync(ApiEndpoints.Commissions + "/recent/" + profile.Email);

                if (httpResult.IsSuccessStatusCode)
                {
                    string recentCommissions = await httpResult.Content.ReadAsStringAsync();
                    Console.WriteLine(recentCommissions);

                    List<Commission> commissions = JsonConvert.DeserializeObject<List<Commission>>(recentCommissions);

                    if (commissions.Count >= 1)
                    {
                        CompletedCommission = commissions[0];
                        CompletedCommissionPreview.DataContext = CompletedCommission;
                    }

                    if (commissions.Count >= 2)
                    {
                        CurrentCommission = commissions[1];
                        CurrentCommissionPreview.DataContext = CurrentCommission;
                    }

                    if (commissions.Count >= 3)
                    {
                        QueuedCommission1 = commissions[2];
                        QueuedCommission1Preview.DataContext = QueuedCommission1;
                    }

                    if (commissions.Count >= 4)
                    {
                        QueuedCommission2 = commissions[3];
                        QueuedCommission2Preview.DataContext = QueuedCommission2;
                    }
                }
                else
                {
                    throw new Exception(message: "Retrieve recent coms failed");
                }
            }
            catch (Exception)
            {
                throw;
            }
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
            if (MainFrame != null && commission != null)
            {
                CommissionView commissionView = new CommissionView(commission.Id);

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
