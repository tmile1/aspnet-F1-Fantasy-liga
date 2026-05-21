namespace F1_Fantasy_liga.Models;

public class DatePickerViewModel
{
    public string Label { get; set; } = string.Empty;
    public string InputName { get; set; } = string.Empty;
    public string InputId { get; set; } = string.Empty;
    public string DateValue { get; set; } = string.Empty;
    public string TimeValue { get; set; } = string.Empty;
    public bool ShowTime { get; set; } = true;
    public string DatePlaceholder { get; set; } = string.Empty;
    public string TimePlaceholder { get; set; } = "HH:mm";
}
