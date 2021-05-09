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
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;

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

        private double _distance;
        public double Distance { get => _distance; set { _distance = value; base.RaisePropertyChangedEvent(); } }

        private string _startpoint;
        public string StartPoint { get => _startpoint; set { _startpoint = value; base.RaisePropertyChangedEvent(); } }

        private string _endpoint;
        public string EndPoint { get => _endpoint; set { _endpoint = value; base.RaisePropertyChangedEvent(); } }

        public AsyncCommand AddTourCommand { get; private set; }
        public IDatabase Database { get; private set; }

        private Tour _selectedTour;
        public Tour SelectedTour { get => _selectedTour; set { _selectedTour = value; base.RaisePropertyChangedEvent(); } }


        private AddTour _addTour;

        public TourPlannerVM()
        {
            this.Tours = new ObservableCollection<Tour>();
            this.AddCommand = new AsyncCommand(OpenAddTourWindow);
            this.RemoveCommand = new AsyncCommand(RemoveTour);
            this.AddTourCommand = new AsyncCommand(AddTour);

            this.Database = DALFactory.GetDatabase();
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
            ITourDAO tourDAO= DALFactory.CreateTourDAO();
            await tourDAO.DeleteTour(SelectedTour.Id);
        }
        public async Task AddTour()
        {
            if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Description)  && this.Distance != default&& string.IsNullOrWhiteSpace(StartPoint)&&string.IsNullOrWhiteSpace(EndPoint))
                return;

            _addTour.Visibility = System.Windows.Visibility.Hidden;//Closing makes the window unusable for reshowing it.

            MapQuest map = new MapQuest();
            await map.GetRoute(StartPoint, EndPoint).ContinueWith(async route =>
            {
                Route route1 = await route;
                if (route1 is null)
                {
                    this.Name = string.Empty;
                    this.Description = string.Empty;
                    this.Distance = 0;
                    this.StartPoint = string.Empty;
                    this.EndPoint = string.Empty;
                    return;
                }
                Guid ig = await map.SaveImage(route1);

                Tour t = new Tour(this.Name, this.Description, Environment.CurrentDirectory + "\\" + ig.ToString() + ".jpg", this.Distance, this.StartPoint, this.EndPoint);
                
                this.Name = string.Empty;
                this.Description = string.Empty;
                this.Distance = 0;
                this.StartPoint = string.Empty;
                this.EndPoint = string.Empty;

                Tours.Add(t);
                ITourDAO tourDAO = DALFactory.CreateTourDAO();
                await tourDAO.AddNewTour(t);
            });
        }
    }
}
