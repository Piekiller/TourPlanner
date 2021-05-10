using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TourPlanner.Models;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using System.Windows;
using System.Windows.Threading;
using QuestPDF.Fluent;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM : ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; }
        public AsyncCommand AddTourWindowCommand { get; private set; }
        public AsyncCommand RemoveCommand { get; private set; }
        public AsyncCommand AddTourCommand { get; private set; }
        public AsyncCommand CreateReportCommand { get; private set; }

        public string _name;
        public string Name { get => _name; set { _name = value; base.RaisePropertyChangedEvent(); } }

        private string _description;
        public string Description { get => _description; set { _description = value; base.RaisePropertyChangedEvent(); } }

        private string _startpoint;
        public string StartPoint { get => _startpoint; set { _startpoint = value; base.RaisePropertyChangedEvent(); } }

        private string _endpoint;
        public string EndPoint { get => _endpoint; set { _endpoint = value; base.RaisePropertyChangedEvent(); } }

        private Tour _selectedTour;
        public Tour SelectedTour { get => _selectedTour; set { _selectedTour = value; base.RaisePropertyChangedEvent(); } }

        public IDatabase Database { get; private set; }


        private AddTour _addTour;

        public TourPlannerVM()
        {
            this.Tours = new ObservableCollection<Tour>();
            this.AddTourWindowCommand = new AsyncCommand(OpenAddTourWindow);
            this.RemoveCommand = new AsyncCommand(RemoveTour);
            this.AddTourCommand = new AsyncCommand(AddTour);
            this.CreateReportCommand = new AsyncCommand(CreateReport);
            this.Database = DALFactory.GetDatabase();
            ITourDAO tourDAO = DALFactory.CreateTourDAO();
            Task.Factory.StartNew(async () =>
            {
                List<Tour> tmp = new List<Tour>(await tourDAO.GetTours());
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    tmp.ForEach(t => Tours.Add(t)); //Necessary because ObservableCollection needs to be used by the UI Thread.
                }));
            });
            this._addTour = new AddTour();
            _addTour.DataContext = this;
        }
        public Task OpenAddTourWindow()
        {
            _addTour.Visibility = Visibility.Visible;
            return Task.CompletedTask;
        }
        public async Task RemoveTour()
        {
            if (SelectedTour is null)
                return;
            Tours.Remove(SelectedTour);
            ITourDAO tourDAO= DALFactory.CreateTourDAO();
            await tourDAO.DeleteTour(SelectedTour.Id);
        }
        public async Task AddTour()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(StartPoint) || string.IsNullOrWhiteSpace(EndPoint))
                return;

            _addTour.Visibility = Visibility.Hidden;//Closing makes the window unusable for reshowing it.

            MapQuest map = new MapQuest();
            Route route = await map.GetRoute(StartPoint, EndPoint);
            if (route is null)
            {
                this.Name = string.Empty;
                this.Description = string.Empty;
                this.StartPoint = string.Empty;
                this.EndPoint = string.Empty;
                return;
            }
            Guid ig = await map.SaveImage(route);
            Tour t = new Tour(this.Name, this.Description, Environment.CurrentDirectory + "\\Images\\" + ig.ToString() + ".jpg", route.distance, this.StartPoint, this.EndPoint);
            Tours.Add(t);
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.StartPoint = string.Empty;
            this.EndPoint = string.Empty;

            ITourDAO tourDAO = DALFactory.CreateTourDAO();
            await tourDAO.AddNewTour(t);
        }
        public Task CreateReport()
        {
            if (SelectedTour is null)
                return Task.CompletedTask;
            TourLogReport document = new TourLogReport(SelectedTour,new List<TourLog> { });
            document.GeneratePdf("Report\\"+Guid.NewGuid()+".pdf");
            return Task.CompletedTask;
        }
    }
}
