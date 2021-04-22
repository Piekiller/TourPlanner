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
using TourPlanner.DataAccessLayer;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM:ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; } 
        public AsyncCommand AddCommand { get; private set; } 
        public AsyncCommand RemoveCommand { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RouteInformation { get; set; }
        public double Distance { get; set; }
        public AsyncCommand AddTourCommand { get; private set; }
        public IDataAccess DataAccess { get; private set; }
        public Tour Selected { get; set; }
        private AddTour _addTour;

        public TourPlannerVM()
        {
            this.Tours= new ObservableCollection<Tour>();
            this.AddCommand = new AsyncCommand(OpenAddTourWindow);
            this.RemoveCommand = new AsyncCommand(async () => Tours.RemoveAt(Tours.Count-1));
            this.AddTourCommand = new AsyncCommand(AddTour);

            this.DataAccess = new InMemoryDB();//Better solution with Dependency injection
            this._addTour = new AddTour();
            _addTour.DataContext = this;
        }
        public Task OpenAddTourWindow()
        {
            _addTour.Show();
            return Task.CompletedTask;
        }
        public async Task AddTour()
        {
            if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Description) && string.IsNullOrWhiteSpace(RouteInformation) && this.Distance != default)
                return;

            Tour t = new Tour(this.Name, this.Description, this.RouteInformation, this.Distance);
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.RouteInformation = string.Empty;
            this.Distance = 0;

            Tours.Add(t);
            DataAccess.SaveTour(t);
            _addTour.Visibility = System.Windows.Visibility.Hidden;//Closing makes the window unusable for reshowing it.
        }
    }
}
