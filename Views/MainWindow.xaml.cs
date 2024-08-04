using GigTracker.Services;
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

namespace GigTracker.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SupabaseConnection supabaseConnection;

        public MainWindow(string username, int userID)
        {
            InitializeComponent();

            txtUserDisplay.Text = "Hi " + username + "!";

            InitializeAsync();
            DisplayTable(userID);
        }

        private async void DisplayTable(int userID)
        {
            ConcertsDataGrid.ItemsSource = null; 
            var concerts = await supabaseConnection.FetchConcerts(userID);
            ConcertsDataGrid.ItemsSource = concerts;
        }


        private async void InitializeAsync()
        {
            try
            {
                supabaseConnection = new SupabaseConnection();
                await supabaseConnection.InitializeClient(); // Ensure this is an async method
                //await supabaseConnection.FetchConcert(); // Ensure this is awaited
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                MessageBox.Show($"Error initializing Supabase connection: {ex.Message}");
            }
        }
    }
}
