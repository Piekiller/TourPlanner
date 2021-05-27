using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TourPlanner.Models;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using System.Windows;
using System.Windows.Threading;
using QuestPDF.Fluent;
using System.IO;
using TourPlanner.Mediator;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Input;
using TourPlanner.ViewModels;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM : ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; } = new();
        public ObservableCollection<TourLog> TourLogs { get; set; } = new();
        private Tour _selectedTour;
        private TourLog _selectedTourLog;
        public Tour SelectedTour { get => _selectedTour; set { _selectedTour = value; base.RaisePropertyChangedEvent(); } }
        public TourLog SelectedTourLog { get => _selectedTourLog; set { _selectedTourLog = value; base.RaisePropertyChangedEvent(); } }

        public RelayCommand AddTourWindowCommand { get; private set; }
        public RelayCommand AddTourLogWindowCommand { get; private set; }
        public AsyncCommand<Image> RemoveTourCommand { get; private set; }
        public AsyncCommand AddTourCommand { get; private set; }
        public AsyncCommand RemoveTourLogCommand { get; private set; }
        public AsyncCommand AddTourLogCommand { get; private set; }
        public AsyncCommand CreateReportCommand { get; private set; }
        public RelayCommand UpdateTourCommand { get; private set; }
        public RelayCommand UpdateTourLogCommand { get; private set; }

        public IDatabase Database { get; private set; }
        private IMediator _mediator;

        private AddTour _addTour;
        private AddTourLog _addTourLog;
        public TourPlannerVM()
        {
            this.AddTourWindowCommand = new (OpenAddTourWindow);
            this.RemoveTourCommand = new (RemoveTour);
            this.CreateReportCommand = new (CreateReport);
            this.AddTourLogWindowCommand = new(OpenAddTourLogWindow);
            this.UpdateTourCommand = new(OpenUpdateTourWindow);
            this.UpdateTourLogCommand = new(OpenUpdateTourLogWindow);

            this.Database = DALFactory.GetDatabase();
            ITourDAO tourDAO = DALFactory.CreateTourDAO();

            Task.Factory.StartNew(async () =>
            {
                List<Tour> tmp = new (await tourDAO.GetTours());
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    tmp.ForEach(t => Tours.Add(t)); //Necessary because ObservableCollection needs to be used by the UI Thread.
                }));
            });

            _mediator = new ConcreteMediator();
            _mediator.RegisterTourHandler(SaveNewTour);
            _mediator.RegisterTourLogHandler(SaveNewTourLog);
        }
        private void OpenAddTourWindow()
        {
            _addTour = new AddTour();
            TourVM tourVM = new(null,_mediator);
            _addTour.DataContext = tourVM;
            _addTour.Visibility = Visibility.Visible;
        }
        private void OpenAddTourLogWindow()
        {
            if (SelectedTour is null)
                return;
            _addTourLog = new AddTourLog();
            TourLogVM tourLogVM = new(SelectedTour);
            _addTourLog.DataContext = tourLogVM;
            tourLogVM.SetMediator(_mediator);
            _addTourLog.Show();
        }
        private async Task RemoveTour(Image image)
        {
            if (SelectedTour is null||image is null)
                return;
            image.Source = null;
            File.Delete(SelectedTour.RouteInformation);
            //ToDo: VS accesses file so it can't be deleted.
            ITourDAO tourDAO= DALFactory.CreateTourDAO();
            await tourDAO.DeleteTour(SelectedTour.Id);
            Tours.Remove(SelectedTour);
            SelectedTour = null;
        }

        private Task CreateReport()
        {
            if (SelectedTour is null)
                return Task.CompletedTask;
            TourLogReport document = new TourLogReport(SelectedTour,TourLogs.Where(log=>log.Tour.Id==SelectedTour.Id).ToList());
            document.GeneratePdf("Report\\"+Guid.NewGuid()+".pdf");
            return Task.CompletedTask;
        }
        private void OpenUpdateTourWindow()
        {
            if (SelectedTour is null)
                return;
            _addTour = new AddTour();
            TourVM tourVM = new(SelectedTour, _mediator);
            _addTour.DataContext = tourVM;
            _addTour.Show();
        }

        private void OpenUpdateTourLogWindow()
        {
            if (SelectedTour is null&&SelectedTourLog is TourLog)
                return;
            _addTourLog = new AddTourLog();
            TourLogVM tourLogVM = new(SelectedTour,SelectedTourLog);
            _addTourLog.DataContext = tourLogVM;
            tourLogVM.SetMediator(_mediator);
            _addTourLog.Show();
        }
            
        public void SaveNewTour(Tour t)
        {
            if(Tours.Contains(t))//Can be found because Equals is overwritten
            {
                Tours.Remove(t);//Remove old object with wrong reference
                Tours.Add(t);//Add old object with right reference.
                return;
            } 
            Tours.Add(t);
                
        }
        public void SaveNewTourLog(TourLog t)
        {
            if (TourLogs.Contains(t))
            {
                TourLogs.Remove(t);
                TourLogs.Add(t);
                return;
            }
            TourLogs.Add(t);
        }
    }
}
