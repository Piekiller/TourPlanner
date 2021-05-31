using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.TourLogFactory
{
    public static class TourLogFactory
    {
        private static ITourLogFactory _instance;
        public static ITourLogFactory GetInstance()
        {
            if (_instance is null)
                _instance = new TourLogFactoryImpl();
            return _instance;
        }
    }
}
