using System;

namespace TourPlanner.Models
{
    public class Tour
    { 
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Route_Information { get; private set; }
        public double Distance { get; private set; }

        public Tour(string name, string description, string route_Information, double distance)
        {
            Name = name;
            Description = description;
            Route_Information = route_Information;
            Distance = distance;
        }
    }
}
