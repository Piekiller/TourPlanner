using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.TourFactory
{
    public static class TourFactory
    {
        private static ITourFactory _instance;

        public static ITourFactory GetInstance()
        {
            if (_instance is null)
                _instance = new TourFactoryImpl();
            return _instance;
        }
    }
}
