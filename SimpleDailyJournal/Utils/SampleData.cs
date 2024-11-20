using SimpleDailyJournal.Services;

namespace SimpleDailyJournal.Utils;

public static class SampleData
{
    public static readonly List<SentimentData> SampleSentimentData =
    [
        new() { Text = "Happy - I had the best day ever!", Label = true },
        new() { Text = "Excited - Feeling thrilled about tomorrow!", Label = true },
        new() { Text = "Happy - Everything went perfectly today.", Label = true },
        new() { Text = "Excited - Can't wait for the weekend trip!", Label = true },

        // Neutral Sentiments
        new() { Text = "Neutral - It was an average day.", Label = false },
        new() { Text = "Neutral - Nothing much happened today.", Label = false },
        new() { Text = "Neutral - A quiet and uneventful day.", Label = false },

        // Negative Sentiments
        new() { Text = "Sad - Feeling down and lonely.", Label = false },
        new() { Text = "Depressed - I am very sad and stressed today.", Label = false },
        new() { Text = "Sad - Things didn't go as planned.", Label = false },
        new() { Text = "Depressed - Struggling to stay positive.", Label = false }
    ];
}