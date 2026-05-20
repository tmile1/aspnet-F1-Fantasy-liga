namespace F1_Fantasy_liga.Models
{
    public class ManageFantasyTeamDriversViewModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public decimal BudgetLimit { get; set; }
        public List<Driver> SelectedDrivers { get; set; } = new();
        public List<Driver> AvailableDrivers { get; set; } = new();
    }
}
