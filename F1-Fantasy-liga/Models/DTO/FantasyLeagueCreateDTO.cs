using System.ComponentModel.DataAnnotations;
using F1_Fantasy_liga.Models.Enums;

namespace F1_Fantasy_liga.Models.DTO
{
    public class FantasyLeagueCreateDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(300)]
        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeagueType LeagueType { get; set; }
    }
}
