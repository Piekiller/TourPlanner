using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.ComponentModel;
namespace TourPlanner.ViewModels
{
    class TourPlannerVM:ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; private set; }
        public TourPlannerVM()
        {
            this.Tours = new ObservableCollection<Tour>();
            this.AddCommand = new RelayCommand(() => Tours.Add(new Tour()));
            this:RemoveCommand = new RelayCommand(() => Tours.RemoveAt(Tours.Count));
        }
    }
}
