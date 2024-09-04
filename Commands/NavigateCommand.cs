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
    /// <summary>
    ///  Implementation for all navigation commands.
    ///  Use a navigation service object with the relevant function to create the viewmodel in App.xaml.cs.
    /// </summary>
    internal class NavigateCommand : CommandBase
    {
        private readonly NavigationService _navigationService;
        public NavigateCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public override void Execute(object? parameter)
        {
            _navigationService.Navigate();
        }
    }
}
