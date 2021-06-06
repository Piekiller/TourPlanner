using Newtonsoft.Json;
using System.Collections.Generic;
using TourPlanner.Models;

namespace TourPlanner.BusinessLayer.SerializationFactory
{
    public class JsonSerializationFactoryImpl : ISerializationFactory
    {
        public IEnumerable<Tour> Deserialize(string data)
            => JsonConvert.DeserializeObject<IEnumerable<Tour>>(data);

        public string Serialize(IEnumerable<Tour> tours)
            => JsonConvert.SerializeObject(tours, Formatting.Indented);
    }
}
