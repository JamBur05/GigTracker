using GigTracker.Interfaces;
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
    /// Delete a selected concert from the users account
    /// </summary>
    internal class DeleteConcertCommand : CommandBase
    {
        private HomeViewModel currentViewModel;
        private Concerts currentConcert;
        private SupabaseConnection supabaseConnection;
        public DeleteConcertCommand(HomeViewModel viewModel) 
        {
            currentViewModel = viewModel;
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
            currentConcert = currentViewModel.SelectedConcert;

            await supabaseConnection.DeleteConcert(currentConcert);
            await currentViewModel.LoadConcerts();
        }
    }
}
