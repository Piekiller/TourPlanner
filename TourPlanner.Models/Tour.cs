using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace TourPlanner.Models
{
    public class Tour
    { 
        public string Name { get; set; }
        public string Description { get; set; }
        public string RouteInformation { get; set; }
        public double Distance { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public Guid Id { get; set; }
        public ObservableCollection<TourLog> Logs { get; set; }
        public Tour()
        {

        }

        public Tour(string name, string description, string routeInformation, double distance, string startpoint, string endpoint)
        {
            Name = name;
            Description = description;
            RouteInformation = routeInformation;
            Distance = distance;
            Id = Guid.NewGuid();
            StartPoint = startpoint;
            EndPoint = endpoint;
        }
        [JsonConstructor]
        public Tour(string name, string description, string route_Information, double distance, string startpoint, string endpoint, Guid id) : this(name, description, route_Information, distance, startpoint, endpoint)
            => Id = id;
        public override string ToString()
        {
            return string.Format(Name + ", " + Description + ", " + Distance + ", " + StartPoint + ", " + EndPoint);
        }

        public override bool Equals(object obj)
        {
            if (obj is null || obj is not Tour)
                return false;
            return Id.Equals((obj as Tour).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
