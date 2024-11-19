using Microsoft.ML;
using Microsoft.ML.Data;

namespace SimpleDailyJournal.Services;

public class SentimentAnalysisService
{
    private readonly MLContext _mlContext;
    private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;
    
    private readonly List<SentimentData> _sampleSentimentData =
    [
        new SentimentData() { Text = "I love this!", Label = true },
        new SentimentData() { Text = "This is amazing!", Label = true },
        new SentimentData() { Text = "I hate this.", Label = false },
        new SentimentData() { Text = "This is terrible.", Label = false }
    ];

    public SentimentAnalysisService()
    {
        // Step 1: Create an ML.NET context (workspace for machine learning tasks)
        _mlContext = new MLContext();

        // Step 2: Define a text featurization step
        // - Converts raw text input into numerical features for ML processing
        var textFeaturization = _mlContext.Transforms.Text.FeaturizeText(
            outputColumnName: "Features",
            inputColumnName: nameof(SentimentData.Text)
        );

        // Step 3: Define the machine learning model training step
        // - This adds a binary classification trainer (Positive/Negative sentiment analysis)
        var sentimentTrainer = _mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
            labelColumnName: "Label",
            featureColumnName: "Features"
        );

        // Step 4: Combine the text featurization and trainer into a pipeline
        var dataPipeline = textFeaturization.Append(sentimentTrainer);

        // Step 5: Initialize a dummy dataset
        // - An empty list to simulate the dataset structure required for pipeline initialization
        var sampleSentimentDataView = _mlContext.Data.LoadFromEnumerable(_sampleSentimentData);

        // Step 6: Fit the pipeline using the dummy dataset
        // - This step "trains" the pipeline with no actual data
        var trainedPipeline = dataPipeline.Fit(sampleSentimentDataView);

        // Step 7: Create a prediction engine
        // - This engine lets us input new text and get sentiment predictions
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(
            trainedPipeline
        );
    }
    
    public string PredictSentiment(string text)
    {
        // Step 1: Prepare the input data
        // - Wrap the raw text into a SentimentData object expected by the prediction engine
        var inputData = new SentimentData
        {
            Text = text
        };

        // Step 2: Use the prediction engine to predict the sentiment
        // - The engine processes the input and produces a SentimentPrediction object
        var predictionResult = _predictionEngine.Predict(inputData);

        // Step 3: Interpret the prediction result
        // - If Prediction is true, sentiment is Positive; otherwise, it's Negative
        var sentimentLabel = predictionResult.Prediction ? "Positive" : "Negative";

        // Step 4: Return the sentiment label
        return sentimentLabel;
    }
}

public class SentimentData
{
    public string Text { get; set; } = string.Empty;    
    
    [ColumnName("Label")]
    public bool Label { get; set; }
}

public class SentimentPrediction
{
    // Map "PredictedLabel" column/prop from the result that the MLModel produces
    // to this property of SentimentPrediction class named "Prediction"
    [ColumnName("PredictedLabel")] public bool Prediction { get; set; }
}