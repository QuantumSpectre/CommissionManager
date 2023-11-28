using CommissionManager.GUI.Models;
using CommissionManager.GUI.Views;
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

namespace CommissionManager.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {
        public Frame MainFrame { get; set; }
        public HttpClientService _httpClientService { get; set; }

        public UserProfile Profile { get; set; }
        public AuthToken AuthToken { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetProperties();

            _MainFrame.Navigate(new SignInView());
        }

        public void SetProperties()
        {
            _httpClientService = new HttpClientService();
            MainFrame = _MainFrame;
            _MainFrame.Navigate(new DashboardView());
        }
    }



}
