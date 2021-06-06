using AsyncAwaitBestPractices.MVVM;
using log4net;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BusinessLayer.MapQuest;
using TourPlanner.BusinessLayer.TourFactory;
using TourPlanner.Mediator;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class TourVM : ViewModelBase
    {
        private IMediator _mediator;
        private string _name;
        private string _description;
        private string _startpoint;
        private string _endpoint;
        private Tour _tour;
        public string Name { get => _name; set { _name = value; base.RaisePropertyChangedEvent(); } }
        public string Description { get => _description; set { _description = value; base.RaisePropertyChangedEvent(); } }
        public string StartPoint { get => _startpoint; set { _startpoint = value; base.RaisePropertyChangedEvent(); } }
        public string EndPoint { get => _endpoint; set { _endpoint = value; base.RaisePropertyChangedEvent(); } }

        public AsyncCommand<Window> AddTourCommand { get; private set; }

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public TourVM(Tour t = null, IMediator mediator = null)
        {
            this._mediator = mediator;
            AddTourCommand = new AsyncCommand<Window>(AddTour);
            if (t is not null)
            {
                _tour = t;
                Name = t.Name;
                Description = t.Description;
                StartPoint = t.StartPoint;
                EndPoint = t.EndPoint;
            }
        }
        private async Task AddTour(Window window)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(StartPoint) || string.IsNullOrWhiteSpace(EndPoint))
                return;

            window.Visibility = Visibility.Hidden;
            Route route = await MapQuest.GetRoute(StartPoint, EndPoint);
            if (route is null)
            {
                this.Name = default;
                this.Description = default;
                this.StartPoint = default;
                this.EndPoint = default;
                _log.Error("GetRoute did not return a Route, proabalby due to bad parameters");
                return;
            }
            string ig = await MapQuest.SaveImage(route);
            Tour t = _tour is not null
                ? (new(this.Name, this.Description, Environment.CurrentDirectory + "\\Images\\" + ig.ToString() + ".jpg", route.distance, this.StartPoint, this.EndPoint, _tour.Id))
                : (new(this.Name, this.Description, Environment.CurrentDirectory + "\\Images\\" + ig.ToString() + ".jpg", route.distance, this.StartPoint, this.EndPoint));

            _log.Debug("New Tour has been added or updated");
            _mediator.Notify(this, t);
            this.Name = default;
            this.Description = default;
            this.StartPoint = default;
            this.EndPoint = default;
            if (_tour is null)
                await TourFactory.GetInstance().CreateItem(t);
            else
                await TourFactory.GetInstance().UpdateItem(t);
        }
        public void SetMediator(IMediator mediator)
            => this._mediator = mediator;
    }
}
