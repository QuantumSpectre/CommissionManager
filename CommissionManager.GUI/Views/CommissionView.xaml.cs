using CommissionManager.GUI.Models;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace CommissionManager.GUI.Views
{
    /// <summary>
    /// Interaction logic for CommissionView.xaml
    /// </summary>
    public partial class CommissionView : Page
    {
        public HttpClientService httpClientService { get; set; }
        public Guid ComID { get; set; }
        public MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

        public CommissionView(Guid comid)
        {
            InitializeComponent();

            ComID = comid;

            httpClientService = mainWindow._httpClientService;
            
            SetDataContext();
        }

        public CommissionView()
        {
            InitializeComponent();

            ComID = Guid.Empty;

            httpClientService = mainWindow._httpClientService;
        }

        private async void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComID != Guid.Empty && Description == null)
            {
                try
                {
                    var httpResult = await httpClientService.GetAsync(ApiEndpoints.Commissions + "/" + ComID);

                    if (httpResult.IsSuccessStatusCode)
                    {
                        string comresult = await httpResult.Content.ReadAsStringAsync();

                        Commission existingCommission = JsonConvert.DeserializeObject<Commission>(comresult);

                        existingCommission.CommissionedDate = DateTime.Parse(CommissionedDate.Text);
                        existingCommission.Deadline = DateTime.Parse(Deadline.Text);
                        existingCommission.Description = Description.Text;
                        existingCommission.ClientId = Guid.Parse(ClientId.Text);
                        existingCommission.Status = Status.Text;

                        await httpClientService.PatchAsync(ApiEndpoints.Commissions + "/" + ComID + "?email=" + existingCommission.Email, existingCommission);

                    }
                    else
                    {
                        throw new Exception(message: "Couldn't save changes :( ");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                try
                {
                    Commission newCommission = new Commission()
                    {
                        Email = mainWindow.Profile.Email,
                        Description = Description.Text,
                        ClientId = string.IsNullOrEmpty(ClientId.Text) ? Guid.Empty : Guid.Parse(ClientId.Text),
                        Status = Status.Text,
                        Id = Guid.NewGuid(),
                    };

                    var result = await httpClientService.PostAsync(ApiEndpoints.Commissions, newCommission);


                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        MessageBox.Show("New commission added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        CommissionView newCommissionView = new CommissionView();

                        DashboardView.Instance = new DashboardView();

                        await DashboardView.Instance.Initialize(mainWindow.Profile, mainWindow.MainFrame);

                        mainWindow.MainFrame.Navigate(newCommissionView);

                    }
                    else
                    {
                        MessageBox.Show($"Error: {result.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    throw;
                }
            }
        }

        private async void DeleteCommissionButton_Click(object sender, RoutedEventArgs e)
        {
            HttpResponseMessage response = await httpClientService.GetAsync(ApiEndpoints.Commissions + "/" + ComID);

            if (response.IsSuccessStatusCode)
            {
                bool deletionSuccessful = await httpClientService.DeleteAsync(ApiEndpoints.Commissions + "/" + ComID + "?email=" + mainWindow.Profile.Email);

                if (deletionSuccessful)
                {
                    MessageBox.Show("Commission deleted successfully!");

                    CommissionView newCommissionView = new CommissionView();

                    DashboardView.Instance = new DashboardView();

                    await DashboardView.Instance.Initialize(mainWindow.Profile, mainWindow.MainFrame);

                    mainWindow.MainFrame.Navigate(newCommissionView);
                }
                else
                {
                    MessageBox.Show("Failed to delete commission.");
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("Commission not found. Cannot delete.");
            }
            else
            {
                MessageBox.Show($"Error checking commission existence. Status code: {response.StatusCode}");
            }
        }

        private async void SetDataContext()
        {
            if (ComID != Guid.Empty)
            {
                try
                {
                    var httpResult = await httpClientService.GetAsync(ApiEndpoints.Commissions + "/" + ComID);

                    if (httpResult.IsSuccessStatusCode)
                    {
                        string comresult = await httpResult.Content.ReadAsStringAsync();

                        Commission selectedCommission = JsonConvert.DeserializeObject<Commission>(comresult);

                        DataContext = selectedCommission;
                    }
                    else
                    {
                        throw new Exception(message: "Failed to retrieve commission details");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                    throw;
                }
            }
        }
    }
}
