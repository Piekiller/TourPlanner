using log4net;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;
using System.Linq;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("TourPlanner.Test")]
namespace TourPlanner.BusinessLayer.TourFactory
{
    internal class TourFactoryImpl : ITourFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(TourFactoryImpl));
        private ITourDAO _tourDao = DALFactory.CreateTourDAO();
        private ITourLogDAO _tourLogDAO = DALFactory.CreateTourLogDAO();
        public async Task CreateItem(Tour t)
        {
            await _tourDao.AddNewTour(t);
            _log.Debug("Create new Tour");
        }

        public async Task DeleteItem(Tour t)
        {
            foreach (var item in t.Logs)
            {
                await _tourLogDAO.DeleteTourLog(item.Id);
            }
            await _tourDao.DeleteTour(t.Id);
            _log.Debug("Delete Tour with id: " + t.Id);
        }

        public virtual async Task<IEnumerable<Tour>> GetItems()
        {
            _log.Debug("Get all Tours");
            IEnumerable<Tour> tours=await _tourDao.GetTours();
            foreach (var item in tours)
            {
                item.Logs=new ObservableCollection<TourLog>(await TourLogFactory.TourLogFactory.GetInstance().GetItem(item));
            }
            return tours;
        }

        public async Task<IEnumerable<Tour>> Search(string itemname, bool caseSensitive=false)
        {
            _log.Debug("Search in tours");
            IEnumerable<Tour> tours = await GetItems();
            if(caseSensitive)
                return tours.Where(tour => tour.Name.Contains(itemname));
            return tours.Where(tour => tour.Name.ToLower().Contains(itemname.ToLower()));
        }

        public async Task UpdateItem(Tour t)
        {
            _log.Debug("Update Tour with id: "+t.Id);
            await _tourDao.UpdateTour(t);
        }
    }
}