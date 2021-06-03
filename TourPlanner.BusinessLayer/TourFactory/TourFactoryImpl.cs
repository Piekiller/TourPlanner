using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;
using System.Linq;
namespace TourPlanner.BusinessLayer.TourFactory
{
    internal class TourFactoryImpl : ITourFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ITourDAO _tourDao = DALFactory.CreateTourDAO();
        public async Task CreateItem(Tour t)
        {
            await _tourDao.AddNewTour(t);
            _log.Debug("Create new Tour");
        }

        public async Task DeleteItem(Tour t)
        {
            await _tourDao.DeleteTour(t.Id);
            _log.Debug("Delete Tour with id: " + t.Id);
        }

        public async Task<IEnumerable<Tour>> GetItems()
        {
            return await _tourDao.GetTours();
        }

        public async Task<IEnumerable<Tour>> Search(string itemname, bool caseSensitive=false)
        {
            IEnumerable<Tour> tours = await GetItems();
            if(caseSensitive)
                return tours.Where(tour => tour.Name.Contains(itemname));
            return tours.Where(tour => tour.Name.ToLower().Contains(itemname.ToLower()));
        }

        public async Task UpdateItem(Tour t)
        {
            await _tourDao.UpdateTour(t);
        }
    }
}