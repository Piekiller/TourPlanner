using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TourPlanner.Models
{
    public class TourLog
    {
        public DateTime Date { get; set; } = new DateTime();
        public string Report { get; set; } = string.Empty;
        public double Distance { get; set; }
        public TimeSpan Time { get; set; } = new TimeSpan();
        public int Rating { get; set; }
        public Guid Id { get; set; } = Guid.NewGuid();
        public double AvgSpeed { get; set; }
        public int BurnedJoule { get; set; }
        public int Difficulty { get; set; }
        public int HeightDelta { get; set; }
        public double MaxSpeed { get; set; }
        public Tour Tour { get; set; } = new Tour();

        public TourLog()
        {

        }
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
            MaxSpeed = maxspeed;
            Tour = tour;
        }

        public TourLog(DateTime date, string report, double distance, TimeSpan time, int rating, double avgSpeed, int burnedJoule, int difficulty, int heightdelta, Tour tour, double maxspeed, Guid id):this(date, report, distance, time, rating, avgSpeed, burnedJoule, difficulty, heightdelta,tour, maxspeed)
            => this.Id = id;
        public override bool Equals(object obj)
        {
            if (obj is null || obj is not TourLog)
                return false;
            return Id.Equals((obj as TourLog).Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }



    }
}
