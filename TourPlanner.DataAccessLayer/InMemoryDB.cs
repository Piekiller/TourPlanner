using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;
using System.Linq;
namespace TourPlanner.DataAccessLayer
{
    public class InMemoryDB:IDataAccess
    {
        List<Tour> tours = new List<Tour>();
        List<TourLog> tourLogs = new List<TourLog>();

        public void DeleteTour(Tour t)  
            => tours.Remove(t);

        public void DeleteTourLog(TourLog tl) 
            => tourLogs.Remove(tl);

        public List<TourLog> GetTourLogs(Tour t)
            => tourLogs.Where(tmp => tmp.Tour.Id == t.Id).ToList();

        public List<Tour> GetTours()
            => tours;

        public void SaveTour(Tour t)
            => tours.Add(t);

        public void SaveTourLog(TourLog tl)
            => tourLogs.Add(tl);

        public void UpdateTour(Tour t)
        {
            tours.Remove(t);//old version of tour gets removed, Remove uses IEquatable to check if its the same object.
            tours.Add(t);//if IEquatable is implemented correctly it should remove the old ver and add the new one
        }

        public void UpdateTourLog(TourLog tl)
        {
            tourLogs.Remove(tl);//same as in UpdateTour
            tourLogs.Add(tl);
        }
    }
}
