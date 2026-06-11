namespace F1_Fantasy_liga.Models.DTO
{
    public class DriverDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public int Number { get; set; }
        public decimal Price { get; set; }
        public ConstructorSummaryDTO? Constructor { get; set; }
    }
}
