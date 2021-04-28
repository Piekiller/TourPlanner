using System;
using System.Collections.Generic;
using System.Text;
using TourPlanner.Models;
using System.Linq;
using System.Threading.Tasks;

namespace TourPlanner.DataAccessLayer
{
    public class InMemoryDB:IDataAccess
    {
        List<Tour> tours = new List<Tour>();
        List<TourLog> tourLogs = new List<TourLog>();

        public event EventHandler<Tour> AddedTour;
        public event EventHandler<TourLog> AddedTourLog;

        public async Task DeleteTour(Tour t)  
            => tours.Remove(t);

        public async Task DeleteTourLog(TourLog tl) 
            => tourLogs.Remove(tl);

        public async Task<List<TourLog>> GetTourLogs(Tour t)
            => tourLogs.Where(tmp => tmp.Tour.Id == t.Id).ToList();

        public async Task<List<Tour>> GetTours()
            => tours;

        public Task SaveTour(Tour t)
        {
            tours.Add(t);
            AddedTour(this,t);
            return Task.CompletedTask;
        }
        public Task SaveTourLog(TourLog tl)
        {
            tourLogs.Add(tl);
            AddedTourLog(this, tl);
            return Task.CompletedTask;
        }

        public Task UpdateTour(Tour t)
        {
            tours.Remove(t);//old version of tour gets removed, Remove uses IEquatable to check if its the same object.
            tours.Add(t);//if IEquatable is implemented correctly it should remove the old ver and add the new one
            return Task.CompletedTask;
        }

        public Task UpdateTourLog(TourLog tl)
        {
            tourLogs.Remove(tl);//same as in UpdateTour
            tourLogs.Add(tl);
            return Task.CompletedTask;
        }
    }
}
