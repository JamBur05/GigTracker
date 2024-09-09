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
    public class AddConcertCommand : CommandBase
    {
        private readonly Users currentUser;
        private SupabaseConnection supabaseConnection;
        private readonly AddConcertViewModel _viewModel;
        private int userID;

        public AddConcertCommand(AddConcertViewModel viewModel)
        {
            _viewModel = viewModel;
            currentUser = ((App)Application.Current).CurrentUser;
            userID = currentUser.id;
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
            string bandName = _viewModel.BandName;
            string venueName = _viewModel.VenueName;
            DateTime date = _viewModel.ConcertDate;
            await supabaseConnection.AddConcert(userID, bandName, venueName, date);
        }
    }
}
