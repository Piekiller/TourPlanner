using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.DataAccessLayer
{
    public class PostreSQLDB : IDataAccess
    {
        public PostreSQLDB()
        {

        }

        public event EventHandler<Tour> AddedTour;
        public event EventHandler<TourLog> AddedTourLog;

        public async Task DeleteTour(Tour t)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteTourLog(TourLog tl)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TourLog>> GetTourLogs(Tour t)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Tour>> GetTours()
        {
            throw new NotImplementedException();
        }

        public async Task SaveTour(Tour t)
        {
            throw new NotImplementedException();
        }

        public async Task SaveTourLog(TourLog tl)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTour(Tour t)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTourLog(TourLog tl)
        {
            throw new NotImplementedException();
        }
    }
}
