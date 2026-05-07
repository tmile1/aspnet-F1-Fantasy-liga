using System.ComponentModel.DataAnnotations;
namespace F1_Fantasy_liga.Models
{
    public class Circuit
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public double Length { get; set; } 
        public int NumberOfLaps { get; set; }
    }
}
