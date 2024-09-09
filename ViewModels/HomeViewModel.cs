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
        private Concerts selectedConcert;
        public ICommand AddConcertNavigateCommand { get; }
        public ICommand DeleteConcertCommand { get; }
        public ICommand UpdateConcertNavigateCommand { get; }

        public ObservableCollection<Concerts> Concerts { get; set; } = new ObservableCollection<Concerts>();

        public Concerts SelectedConcert
        {
            get 
            { 
                return selectedConcert;
            }
            set
            {
                selectedConcert = value;
                OnPropertyChanged(nameof(SelectedConcert));
            }
        }

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public HomeViewModel(Users currentUser, NavigationService addConcertNavigationService, NavigationService updateConcertNavigationService)
        {
            InitializeAsync();
            //MessageBox.Show($"{currentUser.id}");

            // Navigate to Add Concert view
            AddConcertNavigateCommand = new NavigateCommand(addConcertNavigationService);
            DeleteConcertCommand = new DeleteConcertCommand(this);
            UpdateConcertNavigateCommand = new UpdateConcertNavigation(updateConcertNavigationService, this);

            userID = currentUser.id;
            username = currentUser.username;
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

        public async void DeleteConcert(Concerts concert)
        {
            await supabaseConnection.DeleteConcert(concert);
            Concerts.Remove(concert);
        }
    }
}
