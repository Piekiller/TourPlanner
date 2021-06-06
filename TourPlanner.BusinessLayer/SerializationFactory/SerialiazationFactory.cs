using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner.BusinessLayer.SerializationFactory
{
    public class SerialiazationFactory
    {
        private static ISerializationFactory _instance;

        public static ISerializationFactory GetInstance()
        {
            if (_instance is null)
                _instance = new JsonSerialiazationFactoryImpl();
            return _instance;
        }
    }
}
