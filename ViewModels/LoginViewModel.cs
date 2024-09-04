using GigTracker.Commands;
using GigTracker.Events;
using GigTracker.Interfaces;
using GigTracker.Models;
using GigTracker.MVVM;
using GigTracker.Services;
using GigTracker.Stores;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GigTracker.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        SupabaseConnection supabaseConnection;

        private string username;
        private string password;
        private Users currentUser;
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        //public ICommand NavigateCommand { get; } Is this needed?

        public Users CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
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

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public LoginViewModel(NavigationService homeViewNavigationService)
        {
            InitializeAsync();
            //NavigateCommand = new NavigateCommand(homeViewNavigationService); Is this needed?

            LoginCommand = new LoginCommand(this, homeViewNavigationService);
            
            // TODO: IMPLEMENT REGISTER
            RegisterCommand = new RegisterCommand();
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


        private async Task Register()
        {
            await supabaseConnection.CreateAccount(username, password);
        }

        public bool CanExecute(object? parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object? parameter)
        {
            throw new NotImplementedException();
        }
    }
}
