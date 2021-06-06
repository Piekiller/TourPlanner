using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.SerializationFactory
{
    public interface ISerializationFactory
    {
        public string Serialize(IEnumerable<Tour> tours);
        public IEnumerable<Tour> Deserialize(string data);
    }
}
