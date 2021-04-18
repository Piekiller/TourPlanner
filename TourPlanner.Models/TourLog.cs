using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TourPlanner.Models
{
    public class TourLog : IEquatable<TourLog>
    { 
        public DateTime Date { get; private set; }
        public string Report { get; private set; }
        public double Distance { get; private set; }
        public TimeSpan Time { get; private set; }
        public int Rating { get; private set; }
        public Guid Id { get; private set; }
        public double AvgSpeed { get; private set; }
        public int BurnedJoule { get; private set; }
        public int Difficulty { get; private set; }
        public int HeightDelta { get; private set; }
        public Tour Tour { get; private set; }
        public double MaxSpeed { get; private set; }

        public TourLog(DateTime date, string report, double distance, TimeSpan time, int rating, double avgSpeed, int burnedJoule, int difficulty, int heightdelta, Tour tour,double maxspeed)
        {
            Date = date;
            Report = report;
            Distance = distance;
            Time = time;
            Rating = rating;
            Id = Guid.NewGuid();
            AvgSpeed = avgSpeed;
            BurnedJoule = burnedJoule;
            Difficulty = difficulty;
            HeightDelta = heightdelta;
            Tour = tour;
            MaxSpeed = maxspeed;
        }

        public TourLog(DateTime date, string report, double distance, TimeSpan time, int rating, double avgSpeed, int burnedJoule, int difficulty, int heightdelta, Tour tour, double maxspeed, Guid id):this(date, report, distance, time, rating, avgSpeed, burnedJoule, difficulty, heightdelta, tour, maxspeed)
        {
            this.Id = id;
        }
        public bool Equals([AllowNull] TourLog other)
        {
            return Id.Equals(other.Id);
        }



    }
}
