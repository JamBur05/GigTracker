using GigTracker.Commands;
using GigTracker.Interfaces;
using GigTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GigTracker.ViewModels
{
    /// <summary>
    /// ViewModel for adding concerts to the users account in the database.
    /// </summary>
    public class AddConcertViewModel : BaseViewModel
    {
        public ICommand AddConcertCommand { get; }
        public ICommand ReturnCommand { get; }
        private string bandName;
        private string venueName;
        private DateTime concertDate;

        public string BandName
        {
            get { return bandName; }
            set
            {
                bandName = value;
                OnPropertyChanged(nameof(BandName));
            }
        }

        public string VenueName
        {
            get { return venueName; }
            set
            {
                venueName = value;
                OnPropertyChanged(nameof(VenueName));
            }
        }
        public DateTime ConcertDate
        {
            get { return concertDate; }
            set
            {
                concertDate = value;
                OnPropertyChanged(nameof(ConcertDate));
            }
        }
        public AddConcertViewModel(NavigationService returnNavigationService)
        {
            AddConcertCommand = new AddConcertCommand(this);
            ReturnCommand = new NavigateCommand(returnNavigationService);

            ConcertDate = DateTime.Now;
        }
    }
}
