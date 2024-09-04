using GigTracker.Commands;
using GigTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GigTracker.ViewModels
{
    public class AddConcertViewModel : BaseViewModel
    {
        public ICommand AddConcertCommand { get; }

        public AddConcertViewModel()
        {
            AddConcertCommand = new AddConcertCommand();
        }
    }
}
