using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Mediator
{
    public interface IMediator
    {
        void Notify<T>(object sender, T data);
        void RegisterTourHandler(Action<Tour> handler);
        void RegisterTourLogHandler(Action<TourLog> handler);
    }
}
