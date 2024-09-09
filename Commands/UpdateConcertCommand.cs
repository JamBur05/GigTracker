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
    public class UpdateConcertCommand : CommandBase
    {
        private readonly Concerts _selectedConcert;
        private readonly UpdateConcertViewModel _viewModel;
        private Concerts _newConcert;
        private int userID;
        private SupabaseConnection supabaseConnection;

        public UpdateConcertCommand(UpdateConcertViewModel viewModel, Concerts selectedConcert) 
        {
            _viewModel = viewModel;
            _selectedConcert = selectedConcert;
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
            _newConcert = new Concerts()
            {
                BandName = _viewModel.BandName,
                VenueName = _viewModel.VenueName,
                Date = _viewModel.ConcertDate
            };


            await supabaseConnection.UpdateConcert(_selectedConcert, _newConcert);
        }
    }
}
