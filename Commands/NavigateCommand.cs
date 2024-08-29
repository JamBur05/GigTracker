using GigTracker.Interfaces;
using GigTracker.Models;
using GigTracker.Services;
using GigTracker.Stores;
using GigTracker.ViewModels;
using GigTracker.Views;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Commands
{
    internal class NavigateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;

        public NavigateCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        // Users needs changing
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
