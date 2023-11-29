using CommissionManager.GUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CommissionManager.GUI.Views
{
    /// <summary>
    /// Interaction logic for CommissionBookView.xaml
    /// </summary>
    public partial class CommissionBookView : Page
    {
        public List<Commission> Commissions { get; set; }
        public MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public HttpClientService httpClientService;

        public async Task<List<Commission>> GetCommissionsFromDatabase()
        {
            httpClientService = mainWindow._httpClientService;

            List<Commission> commissionList = new List<Commission>();

            try
            {
                var httpResult = await httpClientService.GetAsync(ApiEndpoints.Commissions + "/Byemail/" + mainWindow.Profile.Email);

                if (httpResult.IsSuccessStatusCode)
                {
                    string comresult = await httpResult.Content.ReadAsStringAsync();

                    List<Commission> commissions = JsonConvert.DeserializeObject<List<Commission>>(comresult);

                    foreach (Commission commission in commissions)
                    {
                        Commission newCommission = new Commission
                        {
                            CommissionedDate = DateTime.Parse(commission.CommissionedDate.ToString()),
                            Deadline = DateTime.Parse(commission.Deadline.ToString()),
                            Description = commission.Description,
                            ClientId = Guid.Parse(commission.ClientId.ToString()),
                            Status = commission.Status,
                            Id = Guid.Parse(commission.Id.ToString()),
                        };

                        commissionList.Add(newCommission);
                    }
                }
                else
                {
                    throw new Exception(message: "Failed to retrieve commissions");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw;
            }

            return commissionList;
        }

        public CommissionBookView()
        {
            InitializeComponent();

            httpClientService = mainWindow._httpClientService;

            Commissions = new List<Commission>();

            GetCommissionsFromDatabase();

            CommissionBookView_Loaded();
        }

        private async Task CommissionBookView_Loaded()
        {
            
           await LoadCommissionsAsync();
        }

        private async Task LoadCommissionsAsync()
        {
            var commissions = await GetCommissionsFromDatabase();
            Commissions = commissions;
            CommissionsListView.ItemsSource = Commissions;
        }

        private void AddCommissionClick(object sender, RoutedEventArgs e)
        {
            mainWindow.MainFrame.Navigate(new CommissionView());
        }

        private void CommissionsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CommissionsListView.SelectedItem != null)
            {
                Commission selectedCommission = (Commission)CommissionsListView.SelectedItem;

                mainWindow.MainFrame.Navigate(new CommissionView(selectedCommission.Id));
            }
        }

        public void RefreshView()
        {
            CommissionBookView newView = new CommissionBookView();
            mainWindow.MainFrame.Navigate(newView);
        }
    }
}
