using System;
using System.Diagnostics.CodeAnalysis;

namespace TourPlanner.Models
{
    public class Tour:IEquatable<Tour>
    { 
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string RouteInformation { get; private set; }
        public double Distance { get; private set; }
        public Guid Id { get; private set; }

        public Tour(string name, string description, string routeInformation, double distance)
        {
            Name = name;
            Description = description;
            RouteInformation = routeInformation;
            Distance = distance;
            Id = Guid.NewGuid();
        }

        public Tour(string name, string description, string route_Information, double distance, Guid id) : this(name, description, route_Information, distance)
        {
            Id = id;
        }

        public bool Equals([AllowNull] Tour other)
        {
            return Id.Equals(other.Id);
        }
    }
}
