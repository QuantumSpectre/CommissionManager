using CommissionManager.GUI.Models;
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
    /// Interaction logic for ClientBookView.xaml
    /// </summary>
    public partial class ClientBookView : Page
    {
        List<Client> clients {  get; set; }
        public MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        public HttpClientService httpClientService;

        public ClientBookView()
        {
            InitializeComponent();

            httpClientService = mainWindow._httpClientService;
        }

        public async Task<List<Client>> GetClientsFromDatabaseAsync()
        {
            clients = new List<Client>();

            var httpResult = await httpClientService.GetAsync(ApiEndpoints.Clients + "/Byemail/" + mainWindow.Profile.Email);
        }
    }
}
