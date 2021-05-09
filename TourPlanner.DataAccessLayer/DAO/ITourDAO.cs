using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.Models;
namespace TourPlanner.DataAccessLayer.DAO
{
    public interface ITourDAO
    {
        Task<Tour> FindById(Guid id);
        Task<Tour> AddNewTour(Tour tour);
        Task<IEnumerable<Tour>> GetTours();
        Task DeleteTour(Guid id);
    }
}
