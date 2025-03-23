using Microsoft.ML.Data;

namespace Application.AI_ML_Module
{
    public class StudentFutureCareerPrediction
    {
        [ColumnName("PredictedLabel")]
        public string FutureCareer { get; set; }
    }
}
