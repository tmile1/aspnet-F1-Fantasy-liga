namespace F1_Fantasy_liga.Models
{
    public class Circuit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Length { get; set; } 
        public int NumberOfLaps { get; set; }
    }
}
