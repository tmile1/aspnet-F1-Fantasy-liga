using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Models
{
    public class BuildFantasyTeamViewModel
    {
        public required List<Driver> Drivers { get; set; }
        public decimal BudgetLimit { get; set; } = 100m;
        public int MaxDrivers { get; set; } = 4;
    }
}