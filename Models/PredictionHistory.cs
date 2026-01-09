using System;

namespace Raicu_Eva_Lab4.Models
{
    public class PredictionHistory
    {
        public int Id { get; set; }
        public float PassengerCount { get; set; }
        public float TripTime { get; set; }
        public float TripDistance { get; set; }
        public float PredictedFare { get; set; } // Pretul prezis
        public DateTime PredictionDate { get; set; }
    }
}