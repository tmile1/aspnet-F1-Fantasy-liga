using System.ComponentModel.DataAnnotations;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models.DTO
{
    public class RaceResultUpdateDTO
    {
        [Range(1, 25)]
        public int FinishedPosition { get; set; }

        [Required]
        public int DriverId { get; set; }

        [Required]
        public int RaceId { get; set; }

        [Required]
        public DriverStatus DriverStatus { get; set; }
    }
}
