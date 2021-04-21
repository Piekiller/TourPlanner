using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;
namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public event EventHandler<Tour> AddedTour;
        public event EventHandler<TourLog> AddedTourLog;

        public Task SaveTour(Tour t);
        public Task<List<Tour>> GetTours();
        public Task DeleteTour(Tour t);
        public Task UpdateTour(Tour t);

        public Task SaveTourLog(TourLog tl);
        public Task<List<TourLog>> GetTourLogs(Tour t);
        public Task DeleteTourLog(TourLog tl); 
        public Task UpdateTourLog(TourLog tl);


    }
}
