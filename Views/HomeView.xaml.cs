using GigTracker.Models;
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
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private async void btnNewConcert_Click(object sender, RoutedEventArgs e)
        {
            //frmAddConcert frmAddConcert = new frmAddConcert(viewModel.Username, viewModel.userID);
            //frmAddConcert.Show();
        }

        private async void btnDeleteEntry_Click(object sender, RoutedEventArgs e)
        {
            //var selectedConcert = (Concerts)ConcertsDataGrid.SelectedItem;

            //if (selectedConcert != null)
            //{
            //    viewModel.DeleteConcert(selectedConcert);
            //    await viewModel.LoadConcerts(); // Refresh the data grid
            //}
            //else
            //{
            //    MessageBox.Show("No concert selected!");
            //}
        }

        private void btnUpdateEntry_Click(object sender, RoutedEventArgs e)
        {
            //var selectedConcert = (Concerts)ConcertsDataGrid.SelectedItem;
            //if (selectedConcert != null)
            //{
            //    frmUpdateEntry update = new frmUpdateEntry(selectedConcert, viewModel.Username, viewModel.userID);
            //    update.Show();
            //}
            //else
            //{
            //    MessageBox.Show("No concert selected!");
            //}
        }

        private void btnSpotify_Click(object sender, RoutedEventArgs e)
        {
            //frmSpotify spotify = new frmSpotify();
            //spotify.Show();
        }
    }
}
