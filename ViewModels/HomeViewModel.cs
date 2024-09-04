using GigTracker.Commands;
using GigTracker.Interfaces;
using GigTracker.Models;
using GigTracker.Services;
using Supabase.Gotrue;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GigTracker.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private SupabaseConnection supabaseConnection;
        public int userID { get; private set; }
        private string username;
        public ICommand AddConcertNavigateCommand { get; }

        public ObservableCollection<Concerts> Concerts { get; set; } = new ObservableCollection<Concerts>();

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public HomeViewModel(Users currentUser, NavigationService navigationService)
        {
            InitializeAsync();
            MessageBox.Show($"{currentUser.id}");

            // Navigate to Add Concert view
            AddConcertNavigateCommand = new NavigateCommand(navigationService);

            userID = currentUser.id;
            username = currentUser.username;

            LoadConcerts();
        }

        private async void InitializeAsync()
        {
            try
            {
                supabaseConnection = new SupabaseConnection();
                await supabaseConnection.InitializeClient();
                await LoadConcerts();
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                MessageBox.Show($"Error initializing Supabase connection: {ex.Message}");
            }
        }

        public async Task LoadConcerts()
        {
            Concerts.Clear();
            var concertsList = await supabaseConnection.FetchConcerts(userID);
            foreach (var concert in concertsList)
            {
                Concerts.Add(concert);
            }
        }

        public void DeleteConcert(Concerts concert)
        {
            supabaseConnection.DeleteConcert(concert);
            Concerts.Remove(concert);
        }
    }
}
