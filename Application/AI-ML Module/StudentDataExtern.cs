using Microsoft.ML.Data;

namespace Application.AI_ML_Module
{
    public class StudentDataExtern
    {
        [LoadColumn(1)]
        public string Name { get; set; }

        [LoadColumn(2)]
        public string Gender { get; set; }

        [LoadColumn(3)]
        public float Age { get; set; }

        [LoadColumn(4)]
        public float GPA { get; set; }

        [LoadColumn(5)]
        public string Major { get; set; }

        [LoadColumn(6)]
        public string InterestedDomain { get; set; }

        [LoadColumn(7)]
        public string Projects { get; set; } 

        [LoadColumn(8)]
        public string FutureCareer { get; set; }

        [LoadColumn(9)]
        public string Python { get; set; } 

        [LoadColumn(10)]
        public string SQL { get; set; } 

        [LoadColumn(11)]
        public string Java { get; set; }
    }
}
