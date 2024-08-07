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
using System.Windows.Shapes;

namespace GigTracker.Views
{
    /// <summary>
    /// Interaction logic for frmAddConcert.xaml
    /// </summary>
    public partial class frmAddConcert : Window
    {
        SupabaseConnection supabaseConnection;
        private int userID;
        private string username;
        public frmAddConcert(string username, int userID)
        {
            InitializeComponent();

            this.userID = userID;
            this.username = username;

            InitializeAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string bandName = txtBandName.Text;
            string venueName = txtVenueName.Text;
            DateTime dateTime = txtDate.SelectedDate.Value;
            await supabaseConnection.AddConcert(userID, bandName, venueName, dateTime);

            MainWindow main = new MainWindow(username, userID);
            main.Show();
            this.Close();
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
