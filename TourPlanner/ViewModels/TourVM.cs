using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class TourVM:ViewModelBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; } 
        public string Route_Information { get; private set; } 
        public double Distance { get; private set; }
        public RelayCommand AddTourCommand { get; private set; } 
        public TourVM()
        {
            AddTourCommand = new RelayCommand(AddTour);
        }
        public void AddTour()
        {
            Tour t = new Tour(this.Name, this.Description, this.Route_Information, this.Distance);

        }
    }
}
