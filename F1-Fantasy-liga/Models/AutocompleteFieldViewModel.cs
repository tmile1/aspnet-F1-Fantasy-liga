namespace F1_Fantasy_liga.Models
{
    public class AutocompleteFieldViewModel
    {
        public string Label { get; set; } = string.Empty;
        public string HiddenInputName { get; set; } = string.Empty;
        public string HiddenInputId { get; set; } = string.Empty;
        public string DisplayInputId { get; set; } = string.Empty;
        public string SearchUrl { get; set; } = string.Empty;
        public string Placeholder { get; set; } = string.Empty;
        public int? SelectedId { get; set; }
        public string SelectedText { get; set; } = string.Empty;
    }
}
