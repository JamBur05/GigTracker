using GigTracker.Models;
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
    /// Interaction logic for frmUpdateEntry.xaml
    /// </summary>
    public partial class frmUpdateEntry : Window
    {
        SupabaseConnection supabaseConnection;
        Concerts selectedConcert;
        private string username;
        private int userId;
        public frmUpdateEntry(Concerts selectedConcert, string username, int userId)
        {
            InitializeComponent();
            this.selectedConcert = selectedConcert;
            InitializeAsync();
            this.username = username;
            this.userId = userId;

            txtBandName.Text = selectedConcert.BandName;
            txtVenueName.Text = selectedConcert.VenueName;
            txtDate.SelectedDate = selectedConcert.Date;
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

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            string bandName = txtBandName.Text;
            string venueName = txtVenueName.Text;
            DateTime dateTime = txtDate.SelectedDate.Value;

            var newConcert = new Concerts
            {
                BandName = bandName,
                VenueName = venueName,
                Date = dateTime
            };

            await supabaseConnection.UpdateConcert(selectedConcert, newConcert);
            MainWindow main = new MainWindow(username, userId);
            main.Show();
            this.Close();
        }
    }
}
