using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Mediator
{
    public class ConcreteMediator : IMediator
    {
        private List<Action<Tour>> _tourHandler = new();
        private List<Action<TourLog>> _tourLogHandler = new();
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public void RegisterTourHandler(Action<Tour> handler)
        {
            if (handler is null)
            {
                _log.Error("Tried registering an invalid Handler for Tour");
                throw new ArgumentNullException("Handler cannot be null");
            }
            _tourHandler.Add(handler);
        }
        public void RegisterTourLogHandler(Action<TourLog> handler)
        {
            if (handler is null)
            {
                _log.Error("Tried registering an invalid Handler for TourLog");
                throw new ArgumentNullException("Handler cannot be null");
            }
            _tourLogHandler.Add(handler);
        }
        public void Notify<T>(object sender,T data)
        {
            switch (data)
            {
                case Tour tourdata:
                    _log.Debug($"{sender} notified of new TourData");
                    _tourHandler.ForEach(tmp => tmp(tourdata));
                    break;
                case TourLog tourLogdata:
                    _log.Debug($"{sender} notified of new TourLogData");
                    _tourLogHandler.ForEach(tmp=>tmp(tourLogdata));
                    break;
            }
        }
    }
}
