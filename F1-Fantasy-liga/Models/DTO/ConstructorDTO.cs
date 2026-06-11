namespace F1_Fantasy_liga.Models.DTO
{
    public class ConstructorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime? FoundedDate { get; set; }
    }
}
