using SimpleDailyJournal.Services;

namespace SimpleDailyJournal.Utils
{
    public static class SampleData
    {
        public static readonly List<SentimentData> SampleSentimentData =
        [
            new SentimentData { Text = "Happy - I had the best day ever!", Label = true },
            new SentimentData { Text = "Excited - Feeling thrilled about tomorrow!", Label = true },
            new SentimentData { Text = "Happy - Everything went perfectly today.", Label = true },
            new SentimentData { Text = "Excited - Can't wait for the weekend trip!", Label = true },

            // Neutral Sentiments
            new SentimentData { Text = "Neutral - It was an average day.", Label = false },
            new SentimentData { Text = "Neutral - Nothing much happened today.", Label = false },
            new SentimentData { Text = "Neutral - A quiet and uneventful day.", Label = false },

            // Negative Sentiments
            new SentimentData { Text = "Sad - Feeling down and lonely.", Label = false },
            new SentimentData { Text = "Depressed - I am very sad and stressed today.", Label = false },
            new SentimentData { Text = "Sad - Things didn't go as planned.", Label = false },
            new SentimentData { Text = "Depressed - Struggling to stay positive.", Label = false }
        ];
    }
}