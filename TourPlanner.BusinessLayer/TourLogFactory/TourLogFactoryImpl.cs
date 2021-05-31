using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TourPlanner.DataAccessLayer.Common;
using TourPlanner.DataAccessLayer.DAO;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.TourLogFactory
{
    class TourLogFactoryImpl : ITourLogFactory
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ITourLogDAO _tourLogDAO = DALFactory.CreateTourLogDAO();
        public async Task CreateItem(TourLog t)
        {
            await _tourLogDAO.AddNewTourLog(t);
            _log.Debug("Create new TourLog");
        }

        public async Task DeleteItem(TourLog t)
        {
            await _tourLogDAO.DeleteTourLog(t.Id);
            _log.Debug("Delete Tour with id: " + t.Id);
        }

        public async Task<IEnumerable<TourLog>> GetItem(Tour t)
        {
            return await _tourLogDAO.GetLogForTour(t);
        }

        public async Task UpdateItem(TourLog t)
        {
            await _tourLogDAO.UpdateTourLog(t);
        }
    }
}
