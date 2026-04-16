using F1_Fantasy_liga.Models;

namespace F1_Fantasy_liga.Models
{
    public class BuildFantasyTeamViewModel
    {
        public required List<Driver> Drivers { get; set; }
        public required List<Constructor> Constructors { get; set; }
        public decimal BudgetLimit { get; set; } = 100m;
        public int MaxDrivers { get; set; } = 4;
        public int MaxConstructors { get; set; } = 1;
    }
}