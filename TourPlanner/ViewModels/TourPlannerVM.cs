using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.ComponentModel;
using TourPlanner.Models;
using System.Windows.Input;
using System.Diagnostics;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM:ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; private set; } 
        public ICommand AddCommand { get; private set; } 
        public ICommand RemoveCommand { get; private set; } 
        public TourPlannerVM()
        {
            this.Tours= new ObservableCollection<Tour>();
            this.AddCommand = new RelayCommand(OpenAddTourWindow);
            this.RemoveCommand = new RelayCommand(() => Tours.RemoveAt(Tours.Count-1));
        }
        public void OpenAddTourWindow()
        {
            AddTour addTour = new AddTour();
            addTour.Show();
        }
    }
}
