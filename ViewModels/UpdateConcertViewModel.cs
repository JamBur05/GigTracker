using GigTracker.Commands;
using GigTracker.Interfaces;
using GigTracker.Models;
using GigTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GigTracker.ViewModels
{
    public class UpdateConcertViewModel : BaseViewModel
    {
        private Concerts _selectedConcert;
        public ICommand ReturnCommand { get; }
        public ICommand UpdateConcertCommand { get; }
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

        public UpdateConcertViewModel(NavigationService returnNavigationService, Concerts selectedConcert)
        {
            _selectedConcert = selectedConcert;
            UpdateConcertCommand = new UpdateConcertCommand(this, _selectedConcert);
            ReturnCommand = new NavigateCommand(returnNavigationService);

            // Set text boxes as selected data
            BandName = _selectedConcert.BandName;
            VenueName = _selectedConcert.VenueName;
            ConcertDate = _selectedConcert.Date;
        }
    }
}
