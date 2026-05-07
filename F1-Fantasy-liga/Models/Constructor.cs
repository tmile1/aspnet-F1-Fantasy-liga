using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace F1_Fantasy_liga.Models
{
    public class Constructor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime? FoundedDate { get; set; }

        [NotMapped]
        public int Points => Drivers?.Sum(d => d.Points) ?? 0;
        public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();

    }
}
