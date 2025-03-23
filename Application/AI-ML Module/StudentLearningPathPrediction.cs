using Microsoft.ML.Data;

namespace Application.AI_ML_Module
{
    public class StudentLearningPathPrediction
    {
        [ColumnName("PredictedLabel")]
        public string LearningPath { get; set; }
    }
}
