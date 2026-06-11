namespace F1_Fantasy_liga.Models.DTO
{
    public class RaceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime RaceDate { get; set; }
        public CircuitSummaryDTO? Circuit { get; set; }
    }
}
