using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using System.ComponentModel;
using TourPlanner.Models;
using System.Windows.Input;
using System.Diagnostics;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM:ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; } 
        public AsyncCommand AddCommand { get; private set; } 
        public AsyncCommand RemoveCommand { get; private set; } 
        public TourPlannerVM()
        {
            this.Tours= new ObservableCollection<Tour>();
            this.AddCommand = new AsyncCommand(OpenAddTourWindow);
            this.RemoveCommand = new AsyncCommand(async () => Tours.RemoveAt(Tours.Count-1));
        }
        public Task OpenAddTourWindow()
        {
            AddTour addTour = new AddTour();
            addTour.Show();
            return Task.CompletedTask;
        }
    }
}
