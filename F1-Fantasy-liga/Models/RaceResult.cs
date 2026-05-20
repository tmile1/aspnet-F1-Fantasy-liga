using F1_Fantasy_liga.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace F1_Fantasy_liga.Models
{
    public class RaceResult
    {
        [Key]
        public int Id { get; set; }
        [Range(1, 25)]
        public int FinishedPosition { get; set; }
        [Range(0, 25)]
        public int ScoredPoints { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }
        public virtual Driver? Driver { get; set; }

        [ForeignKey("Race")]
        public int RaceId { get; set; }
        public virtual Race? Race { get; set; }
        [Required]
        public DriverStatus DriverStatus { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
