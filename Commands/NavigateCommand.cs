using GigTracker.Models;
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
        private readonly NavigationStore _navigationStore;
        private Users _currentUser;

        public Users CurrentUser
        {
            set
            {
                _currentUser = value;
            }
        }

        public NavigateCommand(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
        }
        // Users needs changing
        public override void Execute(object? parameter)
        {
            _navigationStore.CurrentViewModel = new HomeViewModel(_currentUser);
        }
    }
}
