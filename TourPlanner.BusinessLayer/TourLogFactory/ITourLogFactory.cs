using System.Collections.Generic;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.TourLogFactory
{
    public interface ITourLogFactory
    {
        Task<IEnumerable<TourLog>> GetItem(Tour t);
        Task CreateItem(TourLog t);
        Task UpdateItem(TourLog t);
        Task DeleteItem(TourLog t);
    }
}
