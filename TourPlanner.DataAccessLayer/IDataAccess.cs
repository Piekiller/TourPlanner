using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;
namespace TourPlanner.DataAccessLayer
{
    public interface IDataAccess
    {
        public void SaveTour(Tour t);
        public List<Tour> GetTours();
        public void DeleteTour(Tour t);
        public void UpdateTour(Tour t);

        public void SaveTourLog(TourLog tl);
        public List<TourLog> GetTourLogs();
        public void DeleteTourLog(TourLog tl); 
        public void UpdateTourLog(TourLog tl);


    }
}
