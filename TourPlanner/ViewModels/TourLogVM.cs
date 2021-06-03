using log4net;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer.TourLogFactory;
using TourPlanner.Mediator;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class TourLogVM : ViewModelBase
    {
        private IMediator _mediator;
        private string _date;
        private string _report;
        private double _distance;
        private TimeSpan _time;
        private int _rating;
        private int _burnedJoule;
        private int _difficulty;
        private int _heightDelta;
        private double _maxSpeed;
        private Tour _tour;
        private TourLog _tourLog;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string Date { get => _date; set { _date = value; base.RaisePropertyChangedEvent(); } }
        public string Report { get => _report; set { _report = value; base.RaisePropertyChangedEvent(); } }
        public double Distance { get => _distance; set { _distance = value; base.RaisePropertyChangedEvent(); } }
        public TimeSpan Time { get => _time; set { _time = value; base.RaisePropertyChangedEvent(); } }
        public int Rating { get => _rating; set { _rating = value; base.RaisePropertyChangedEvent(); } }
        public int BurnedJoule { get => _burnedJoule; set { _burnedJoule = value; base.RaisePropertyChangedEvent(); } }
        public int Difficulty { get => _difficulty; set { _difficulty = value; base.RaisePropertyChangedEvent(); } }
        public int HeightDelta { get => _heightDelta; set { _heightDelta = value; base.RaisePropertyChangedEvent(); } }
        public double MaxSpeed { get => _maxSpeed; set { _maxSpeed = value; base.RaisePropertyChangedEvent(); } }

        public ICommand AddTourLogCommand { get; private set; }
        public TourLogVM(Tour t, TourLog tourLog=null)
        {
            _tour = t;
            _tourLog = tourLog;
            AddTourLogCommand = new RelayCommand<Window>(AddTourLog);
            if(tourLog is not null)
            {
                Date = tourLog.Date.ToString();
                Report = tourLog.Report;
                Distance = tourLog.Distance;
                Time = tourLog.Time;
                Rating = tourLog.Rating;
                BurnedJoule = tourLog.BurnedJoule;
                Difficulty = tourLog.Difficulty;
                HeightDelta = tourLog.HeightDelta;
                MaxSpeed = tourLog.MaxSpeed;
            }
            else
                Date = DateTime.Now.ToString();
        }
        public void SetMediator(IMediator mediator)
            => this._mediator = mediator;
        private void AddTourLog(Window window)
        {
            if (Date == default || string.IsNullOrWhiteSpace(Report) || Distance == default || Time == default
                ||Rating == default || BurnedJoule == default || Difficulty == default || HeightDelta == default || MaxSpeed == default)
                return;
            
            window.Hide();
            TourLog t;
            if (_tourLog is not null)
                t = new(DateTime.Parse(Date), Report, Distance, Time, Rating, Distance / Time.TotalHours, BurnedJoule, Difficulty, HeightDelta, _tour, MaxSpeed, _tourLog.Id);
            else
                t = new (DateTime.Parse(Date), Report,Distance, Time, Rating, Distance / Time.TotalHours,BurnedJoule,Difficulty,HeightDelta, _tour, MaxSpeed);
            _mediator.Notify(this, t);
            _log.Debug("New TourLog has been added");
            this.Date = default;
            this.Report = default;
            this.Distance = default;
            this.Time = default;
            this.Rating = default;
            this.BurnedJoule = default;
            this.Difficulty = default;
            this.HeightDelta = default;
            this.MaxSpeed = default;
            if (_tourLog is null)
                TourLogFactory.GetInstance().CreateItem(t);
            else
                TourLogFactory.GetInstance().UpdateItem(t);
        }

    }
}
