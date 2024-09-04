using GigTracker.Events;
using GigTracker.Models;
using GigTracker.Services;
using GigTracker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GigTracker.Commands
{
    /// <summary>
    /// Command for processing logins, will navigate to home viewmodel if successful
    /// </summary>
    internal class LoginCommand : CommandBase
    {
        private readonly LoginViewModel _viewModel;
        private readonly NavigationService _homeViewNavigationService;
        private SupabaseConnection supabaseConnection;
        public event EventHandler<LoginSuccessEventArgs>? LoginSuccess;

        public LoginCommand(LoginViewModel viewModel, NavigationService homeViewNavigationService)
        {
            _viewModel = viewModel;
            _homeViewNavigationService = homeViewNavigationService;
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                supabaseConnection = new SupabaseConnection();
                await supabaseConnection.InitializeClient(); 
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                MessageBox.Show($"Error initializing Supabase connection: {ex.Message}");
            }
        }

        public override async void Execute(object? parameter)
        {
            Users _currentUser = await supabaseConnection.ValidUser(_viewModel.Username, _viewModel.Password);

            if (_currentUser != null)
            {
                _viewModel.CurrentUser = _currentUser;
                ((App)Application.Current).CurrentUser = _currentUser;

                _homeViewNavigationService.Navigate();
            }
            else
            {
                MessageBox.Show("No user found");
            }

        }
    }

}
