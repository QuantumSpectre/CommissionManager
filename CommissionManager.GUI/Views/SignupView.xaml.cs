using CommissionManager.GUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Interaction logic for SignupView.xaml
    /// </summary>
    public partial class SignupView : Page
    {
        public HttpClientService httpClientService { get; set; }

        public SignupView()
        {
            InitializeComponent();

            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            httpClientService = mainWindow._httpClientService;
        }

        private async void RegistrationButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            var mainFrame = mainWindow.MainFrame;

            if (httpClientService == null)
            {
                //error handling
                return;
            }

            if (PasswordBox.Password == ConfirmPasswordBox.Password)
            {
                UserProfile userProfile = new UserProfile()
                {
                    Username = UsernameTextBox.Text,
                    Email = EmailTextBox.Text,
                    password = PasswordBox.Password,
                };

                try
                {
                    var response = await httpClientService.PostAsync(ApiEndpoints.Users, userProfile);

                    if (response.IsSuccessStatusCode)
                    {
                        //Regestration success!
                        MessageBox.Show("Regestration Successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        mainWindow.AuthToken = new AuthToken(EmailTextBox.Text, true);

                        mainFrame.Navigate(new SignInView());
                    }
                    else
                    {
                        //Registration failure
                        MessageBox.Show($"Registration failed. Status code: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    //Logging
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else { return; }
            }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegistrationButton_Clicked(sender, e);
            }
        }
    }
    }
