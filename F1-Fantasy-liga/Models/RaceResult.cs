using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models
{
    public class RaceResult
    {
        public int Id { get; set; }
        public int FinishedPosition { get; set; }
        public int ScoredPoints { get; set; }
        public int DriverId { get; set; }
        public Driver Driver { get; set; }
        public int RaceId { get; set; }
        public Race Race { get; set; }
        public DriverStatus DriverStatus { get; set; }
    }
}
