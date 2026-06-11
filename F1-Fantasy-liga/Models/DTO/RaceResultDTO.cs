using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models.DTO
{
    public class RaceResultDTO
    {
        public int Id { get; set; }
        public int FinishedPosition { get; set; }
        public int ScoredPoints { get; set; }
        public DriverStatus DriverStatus { get; set; }
        public DriverSummaryDTO? Driver { get; set; }
        public RaceSummaryDTO? Race { get; set; }
    }
}
