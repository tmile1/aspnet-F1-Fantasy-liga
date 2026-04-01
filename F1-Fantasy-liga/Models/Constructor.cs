namespace F1_Fantasy_liga.Models
{
    public class Constructor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public DateTime? FoundedDate { get; set; }
        public int Points => Drivers.Sum(d => d.Points);
        public List<Driver> Drivers { get; set; } = new List<Driver>();

    }
}
