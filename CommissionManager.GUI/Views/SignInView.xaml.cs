using CommissionManager.GUI.Models;
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
    /// Interaction logic for SignInView.xaml
    /// </summary>
    public partial class SignInView : Page
    {
        public HttpClientService httpClientService { get; set; }
        public Frame? mainFrame { get; set; }
        public UserProfile profile { get; set; }

        public SignInView()
        {
            InitializeComponent();

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow?.MainFrame;
        }

        private async void SignInButtonClicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;

            httpClientService = mainWindow._httpClientService;

            if (httpClientService == null)
            {
                return;
            }

            //collect values from view
            UserAuthenticationRequest userRequest = new UserAuthenticationRequest()
            {
                Email = EmailTextBox.Text,
                Password = PasswordBox.Password,

               
            };

            try
            {
                var response = await httpClientService.PostAsync(ApiEndpoints.VerifyPassword, userRequest );

                if (response.IsSuccessStatusCode)
                {
                    // User authentication succeeded
                    MessageBox.Show("Sign-in Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    mainWindow.AuthToken = new AuthToken(EmailTextBox.Text, true);

                    var userProfileResponse = await httpClientService.GetAsync(ApiEndpoints.Users + "/?email=" + userRequest.Email);

                    if(userProfileResponse.IsSuccessStatusCode)
                    {
                        string userProfileJson = await userProfileResponse.Content.ReadAsStringAsync();

                        UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(userProfileJson);

                        mainWindow.Profile = userProfile;

                        DashboardView dashboardView = new DashboardView();

                        dashboardView.Initialize(userProfile, mainFrame);

                        mainFrame.Navigate(dashboardView);
                    }
                }
                else
                {
                    // User authentication failed
                    MessageBox.Show($"Sign-in failed. Status code: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Logging
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterNewUser(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainFrame = mainWindow.MainFrame;

            if (mainWindow != null)
            {
                mainFrame.Navigate(new SignupView());
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SignInButtonClicked(sender, e);
            }
        }
    }
}

