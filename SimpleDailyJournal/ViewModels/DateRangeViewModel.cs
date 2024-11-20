using SimpleDailyJournal.Models;

namespace SimpleDailyJournal.ViewModels;

public class DateRangeViewModel
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public MoodType? SelectedMood { get; set; } // if null, means "All Moods"
}