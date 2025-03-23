using Microsoft.ML.Data;

namespace Application.AI_ML_Module
{
    public class StudentAverageGradePrediction
    {
        [ColumnName("Score")]
        public float AverageGrade { get; set; }
    }
}
