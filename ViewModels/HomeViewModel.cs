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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GigTracker.ViewModels
{
    /// <summary>
    /// ViewModel for home page, displays database view and allows user to select options for modifying database etc...
    /// </summary>
    public class HomeViewModel : BaseViewModel
    {
        private SupabaseConnection supabaseConnection;
        public int userID { get; private set; }
        private string username;
        private string _displayText;
        private string _spotifyText;
        private string spotifyAuthKey;
        private Concerts selectedConcert;
        private SpotifyService spotifyService;
        public ICommand AddConcertNavigateCommand { get; }
        public ICommand DeleteConcertCommand { get; }
        public ICommand UpdateConcertNavigateCommand { get; }
        public ICommand ConnectSpotifyCommand { get; }

        public ObservableCollection<Concerts> Concerts { get; set; } = new ObservableCollection<Concerts>();

        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                _displayText = value;
                OnPropertyChanged(nameof(DisplayText)); 
            }
        }
        public string SpotifyText
        {
            get { return _spotifyText; }
            set
            {
                _spotifyText = value;
                OnPropertyChanged(nameof(SpotifyText));
            }
        }
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
            ConnectSpotifyCommand = new ConnectSpotifyCommand(this);

            // Set welcome message
            DisplayText = $"Hi {currentUser.username}!";
            
            userID = currentUser.id;
            username = currentUser.username;
        }

        public async void StartSpotifyService(string authKey)
        {
            int counter = 1;

            spotifyAuthKey = authKey;
            spotifyService = new SpotifyService(spotifyAuthKey);
            var topArtists = await spotifyService.GetTopArtistsAsync();

            SpotifyText = "Top 10 Artists:";

            foreach (var artist in topArtists.Items)
            {
                SpotifyText += "\n";
                SpotifyText += $"{counter++}. {artist.Name}";
            }

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
