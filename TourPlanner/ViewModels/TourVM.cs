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
    class TourVM:ViewModelBase
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
        public TourVM(Tour t=null, IMediator mediator = null)
        {
            this._mediator = mediator;
            AddTourCommand = new AsyncCommand<Window>(AddTour);
            if(t is not null)
            {
                _tour = t;
                Name = t.Name;
                Description = t.Description;
                StartPoint = t.StartPoint;
                EndPoint = t.EndPoint;
            }
        }

        public async Task AddTour(Window window)
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
                return;
            }
            Guid ig = await MapQuest.SaveImage(route);
            Tour t;
            if (_tour is not null)
                t = new(this.Name, this.Description, Environment.CurrentDirectory + "\\Images\\" + ig.ToString() + ".jpg", route.distance, this.StartPoint, this.EndPoint, _tour.Id);
            else
                t = new(this.Name, this.Description, Environment.CurrentDirectory + "\\Images\\" + ig.ToString() + ".jpg", route.distance, this.StartPoint, this.EndPoint);
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
