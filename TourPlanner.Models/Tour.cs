using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TourPlanner.Models
{
    public class Tour:IEquatable<Tour>
    { 
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string RouteInformation { get; private set; }
        public double Distance { get; private set; }
        public string StartPoint { get; private set; }
        public string EndPoint { get; private set; }
        public Guid Id { get; private set; }
        public List<TourLog> Logs { get; private set; }
        public Tour(string name, string description, string routeInformation, double distance, string startpoint,string endpoint,List<TourLog> logs)
        {
            Name = name;
            Description = description;
            RouteInformation = routeInformation;
            Distance = distance;
            Id = Guid.NewGuid();
            StartPoint = startpoint;
            EndPoint = endpoint;
            Logs = logs;
        }

        public Tour(string name, string description, string route_Information, double distance, string startpoint, string endpoint, List<TourLog> logs, Guid id) : this(name, description, route_Information, distance, startpoint, endpoint,logs)
            => Id = id;

        public bool Equals([AllowNull] Tour other)
        {
            if (other is null)
                return false;
            return Id.Equals(other.Id);
        }
    }
}
