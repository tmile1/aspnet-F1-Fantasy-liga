namespace F1_Fantasy_liga.Models
{
    public class Race
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RaceDate { get; set; }
        public int CircuitId { get; set; }
        public Circuit Circuit { get; set; }
        public List<RaceResult> RaceResults { get; set; } = new List<RaceResult>();
    }
}
