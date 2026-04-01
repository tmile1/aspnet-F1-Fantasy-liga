namespace F1_Fantasy_liga.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public int Points { get; set; }
        public int ConstructorId { get; set; }
        public Constructor Constructor { get; set; }
        public List<RaceResult> RaceResults { get; set; } = new List<RaceResult>();
    }
}
