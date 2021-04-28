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
using TourPlanner.BusinessLayer;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM : ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public AsyncCommand AddCommand { get; private set; }
        public AsyncCommand RemoveCommand { get; private set; }

        public string _name;
        public string Name { get => _name; set { _name = value; base.RaisePropertyChangedEvent(); } }

        private string _description;
        public string Description { get => _description; set { _description = value; base.RaisePropertyChangedEvent(); } }

        private string _routeInformation;
        public string RouteInformation { get => _routeInformation; set { _routeInformation = value; base.RaisePropertyChangedEvent(); } }

        private double _distance;
        public double Distance { get => _distance; set { _distance = value; base.RaisePropertyChangedEvent(); } }

        public AsyncCommand AddTourCommand { get; private set; }
        public IDataAccess DataAccess { get; private set; }

        private Tour _selectedTour;
        public Tour SelectedTour { get => _selectedTour; set { _selectedTour = value; base.RaisePropertyChangedEvent(); } }


        private AddTour _addTour;

        public TourPlannerVM()
        {
            this.Tours = new ObservableCollection<Tour>();
            this.AddCommand = new AsyncCommand(OpenAddTourWindow);
            this.RemoveCommand = new AsyncCommand(RemoveTour);
            this.AddTourCommand = new AsyncCommand(AddTour);

            this.DataAccess = new InMemoryDB();//Better solution with Dependency injection
            this._addTour = new AddTour();
            _addTour.DataContext = this;
        }
        public Task OpenAddTourWindow()
        {
            _addTour.Visibility = System.Windows.Visibility.Visible;
            return Task.CompletedTask;
        }
        public async Task RemoveTour()
        {
            Tours.Remove(SelectedTour);
            await DataAccess.DeleteTour(SelectedTour);
        }
        public async Task AddTour()
        {
            if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Description) && string.IsNullOrWhiteSpace(RouteInformation) && this.Distance != default)
                return;

            _addTour.Visibility = System.Windows.Visibility.Hidden;//Closing makes the window unusable for reshowing it.

            MapQuest map = new MapQuest();
            Route route = await map.GetRoute("Vienna", "Salzburg");
            Guid ig = await map.SaveImage(route);

            Tour t = new Tour(this.Name, this.Description, Environment.CurrentDirectory +"\\"+ ig.ToString()+".jpg", this.Distance);
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.RouteInformation = string.Empty;
            this.Distance = 0;

            Tours.Add(t);
            await DataAccess.SaveTour(t);
            
        }
    }
}
