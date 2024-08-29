using GigTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigTracker.Commands
{
    internal class AddConcertNavigateCommand : CommandBase
    {
        private readonly NavigationService _addConcertViewNavigationService;

        public AddConcertNavigateCommand(NavigationService addConcertViewNavigationService) 
        {
            _addConcertViewNavigationService = addConcertViewNavigationService;
        }

        public override void Execute(object? parameter)
        {
            _addConcertViewNavigationService.Navigate();
        }
    }
}
