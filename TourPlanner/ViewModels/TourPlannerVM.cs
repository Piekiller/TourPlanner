using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TourPlanner.Models;
using AsyncAwaitBestPractices.MVVM;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer;
using System.Windows;
using System.Windows.Threading;
using QuestPDF.Fluent;
using System.IO;
using TourPlanner.Mediator;
using System.Windows.Controls;
using System.Linq;
using TourPlanner.BusinessLayer.TourFactory;
using TourPlanner.BusinessLayer.TourLogFactory;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace TourPlanner.ViewModels
{
    public class TourPlannerVM : ViewModelBase
    {
        public ObservableCollection<Tour> Tours { get; set; } = new();
        public ObservableCollection<Tour> SearchedTours { get; set; } = new();
        private Tour _selectedTour;
        private TourLog _selectedTourLog;

        public Tour SelectedTour { get => _selectedTour; set { _selectedTour = value; base.RaisePropertyChangedEvent(); } }
        public TourLog SelectedTourLog { get => _selectedTourLog; set { _selectedTourLog = value; base.RaisePropertyChangedEvent(); } }
        public SearchTourVM TourVm { get; set; } 

        public RelayCommand AddTourWindowCommand { get; private set; }
        public RelayCommand AddTourLogWindowCommand { get; private set; }
        public AsyncCommand<Image> RemoveTourCommand { get; private set; }
        public AsyncCommand AddTourCommand { get; private set; }
        public AsyncCommand RemoveTourLogCommand { get; private set; }
        public AsyncCommand AddTourLogCommand { get; private set; }
        public AsyncCommand CreateReportCommand { get; private set; }
        public RelayCommand UpdateTourCommand { get; private set; }
        public RelayCommand UpdateTourLogCommand { get; private set; }
        public AsyncCommand ImportCommand { get; private set; }
        public AsyncCommand ExportCommand { get; private set; }

        private IMediator _mediator;
        private AddTour _addTour;
        private AddTourLog _addTourLog;

        public TourPlannerVM()
        {
            this.AddTourWindowCommand = new(OpenAddTourWindow);
            this.RemoveTourCommand = new(RemoveTour);
            this.RemoveTourLogCommand = new(RemoveTourLog);
            this.CreateReportCommand = new(CreateReport);
            this.AddTourLogWindowCommand = new(OpenAddTourLogWindow);
            this.UpdateTourCommand = new(OpenUpdateTourWindow);
            this.UpdateTourLogCommand = new(OpenUpdateTourLogWindow);
            this.ImportCommand = new(Import);
            this.ExportCommand = new(Export);

            Task.Factory.StartNew(async () =>
            {
                List<Tour> tmp = new(await TourFactory.GetInstance().GetItems());
                await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    tmp.ForEach(t =>
                    {
                        Tours.Add(t);
                    }); //Necessary because ObservableCollection needs to be used by the UI Thread.
                    foreach (var item in Tours)
                        SearchedTours.Add(item);
                    TourVm = new SearchTourVM(SearchedTours.ToList());
                }));
            });

            _mediator = new ConcreteMediator();
            _mediator.RegisterTourHandler(SaveNewTour);
            _mediator.RegisterTourLogHandler(SaveNewTourLog);
        }
        private void OpenAddTourWindow()
        {
            _addTour = new AddTour();
            TourVM tourVM = new(null, _mediator);
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
            if (SelectedTour is null || image is null)
                return;
            image.Source = null;
            await TourFactory.GetInstance().DeleteItem(SelectedTour);
            Tours.Remove(SelectedTour);
            SelectedTour = null;
        }
        private async Task RemoveTourLog()
        {
            if (SelectedTourLog is null || SelectedTour is null)
                return;
            await TourLogFactory.GetInstance().DeleteItem(SelectedTourLog);
            SelectedTour.Logs.Remove(SelectedTourLog);
            SelectedTourLog = null;
        }
        private Task CreateReport()
        {
            if (SelectedTour is null)
                return Task.CompletedTask;
            TourLogReport document = new(SelectedTour, SelectedTour.Logs.ToList());
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.DefaultExt = "*.pdf";
            saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() is not null and true)
            {
                document.GeneratePdf(saveFileDialog.FileName);
            }
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
            if (SelectedTour is null && SelectedTourLog is TourLog)
                return;
            _addTourLog = new AddTourLog();
            TourLogVM tourLogVM = new(SelectedTour, SelectedTourLog);
            _addTourLog.DataContext = tourLogVM;
            tourLogVM.SetMediator(_mediator);
            _addTourLog.Show();
        }
        private async Task Import()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "json files (*.json)|*.json";
            string data = "";
            if (openFileDialog.ShowDialog() is not null and true)
            {
                data = await File.ReadAllTextAsync(openFileDialog.FileName);
                List<Tour> tours = JsonConvert.DeserializeObject<Tour[]>(data).ToList();
                Tours.Clear();
                tours.ForEach(tour => Tours.Add(tour));
            }
        }
        private async Task Export()
        {
            SaveFileDialog saveFileDialog = new();
            saveFileDialog.DefaultExt = "*.json";
            saveFileDialog.Filter = "json files (*.json)|*.json";
            if (saveFileDialog.ShowDialog() is not null and true)
            {
                using Stream stream = saveFileDialog.OpenFile();
                using StreamWriter sw = new(stream);
                string data = JsonConvert.SerializeObject(Tours, Formatting.Indented);
                await sw.WriteLineAsync(data);
                sw.Flush();
            }
        }
        public void SaveNewTour(Tour t)
        {
            if (Tours.Contains(t))//Can be found because Equals is overwritten
            {
                Tours.Remove(t);//Remove old object with wrong reference
                Tours.Add(t);//Add old object with right reference.
                return;
            }
            Tours.Add(t);
        }
        public void SaveNewTourLog(TourLog t)
        {
            if (SelectedTour is null)
                return;
            if (SelectedTour.Logs.Contains(t))
            {
                SelectedTour.Logs.Remove(t);
                SelectedTour.Logs.Add(t);
                return;
            }
            SelectedTour.Logs.Add(t);
        }
    }
}
