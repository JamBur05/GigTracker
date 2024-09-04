using GigTracker.Models;
using GigTracker.Services;
using GigTracker.Stores;
using GigTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace GigTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;

        private Users _currentUser;

        public Users CurrentUser
        {
            set
            {
                _currentUser = value;
            }
            get 
            {
                return _currentUser; 
            }
        }

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore.CurrentViewModel = new LoginViewModel(new NavigationService(_navigationStore, CreateHomeViewModel)); 
            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private HomeViewModel CreateHomeViewModel()
        {
            var addConcertNavigationService = new NavigationService(_navigationStore, CreateAddConcertViewModel);
            return new HomeViewModel(_currentUser, addConcertNavigationService);
        }
        private AddConcertViewModel CreateAddConcertViewModel()
        {
            var returnHomeNavigationService = new NavigationService(_navigationStore, CreateHomeViewModel);
            return new AddConcertViewModel(returnHomeNavigationService);
        }

    }
}
