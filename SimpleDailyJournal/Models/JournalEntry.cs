using System.ComponentModel.DataAnnotations;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace SimpleDailyJournal.Models;

public enum MoodType
{
    Excited,
    Happy,
    Neutral,
    Sad,
    Depressed
}

public class JournalEntry
{
    public int Id { get; set; }

    [Required(ErrorMessage = "DateTime is required.")]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Content is required.")]
    [StringLength(500, ErrorMessage = "Content cannot exceed 500 characters.")]
    public string Content { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mood is required.")]
    public MoodType Mood { get; set; }

    [StringLength(10, ErrorMessage = "Sentiment cannot exceed 10 characters.")]
    public string? Sentiment { get; set; } = null;
}