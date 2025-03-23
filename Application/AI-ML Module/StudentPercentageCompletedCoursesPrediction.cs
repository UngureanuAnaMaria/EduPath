using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AI_ML_Module
{
    public class StudentPercentageCompletedCoursesPrediction
    {

        [ColumnName("Score")]
        public float PercentageCompletedCourses { get; set; }
    }
}
