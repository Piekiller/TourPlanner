using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.Models;
namespace TourPlanner.DataAccessLayer.DAO
{
    public interface ITourLogDAO
    {
        Task<TourLog> FindById(Guid id);
        Task<TourLog> AddNewTourLog(TourLog log);
        Task<IEnumerable<TourLog>> GetLogForTour(Tour tour);
        Task DeleteTourLog(Guid id);
        Task UpdateTourLog(TourLog log);
    }
}
