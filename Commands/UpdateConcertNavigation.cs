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
    public class UpdateConcertNavigation : CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly HomeViewModel _homeViewModel;
        public UpdateConcertNavigation(NavigationService navigationService, HomeViewModel homeViewModel)
        {
            _navigationService = navigationService;
            _homeViewModel = homeViewModel;
        }
        public override void Execute(object? parameter)
        {
            var selectedConcert = _homeViewModel.SelectedConcert;
            ((App)Application.Current).SelectedConcert = selectedConcert;
            _navigationService.Navigate();
        }
    }
}
