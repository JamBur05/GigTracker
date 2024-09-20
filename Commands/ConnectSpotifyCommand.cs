using GigTracker.Services;
using GigTracker.ViewModels;
using GigTracker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GigTracker.Commands
{
    public class ConnectSpotifyCommand : CommandBase
    {
        private readonly OAuthService _oauthService;
        private HomeViewModel _homeViewModel;
        public ConnectSpotifyCommand(HomeViewModel homeViewModel) 
        {
            _oauthService = new OAuthService();
            _homeViewModel = homeViewModel;
        }

        public override async void Execute(object? parameter)
        {
            var tokenResponse = await _oauthService.GetAuthCodeAsync();
            _homeViewModel.StartSpotifyService(tokenResponse.AccessToken);
        }
    }
}
