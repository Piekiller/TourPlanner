using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.TourFactory
{
    public interface ITourFactory
    {
        Task<IEnumerable<Tour>> GetItems();
        Task<IEnumerable<Tour>> Search(string itemname, bool caseSensitive=false);
        Task CreateItem(Tour t);
        Task UpdateItem(Tour t);
        Task DeleteItem(Tour t);
    }
}
