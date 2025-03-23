using Application.AI_ML_Module;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntelligentOnlineLearningManagementSystem.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StudentPredictionsController : ControllerBase
    {
        private readonly StudentPerformancePredictionModel studentPerformancePredictionModel;

        public StudentPredictionsController(ApplicationDbContext context)
        {
            this.studentPerformancePredictionModel = new StudentPerformancePredictionModel();
            var sampleData = StudentDataGenerator.GenerateSampleData();
            if (sampleData == null || !sampleData.Any())
            {
                throw new InvalidOperationException("Datele de antrenament sunt goale.");
            }

            foreach (var student in sampleData)
            {
                if (string.IsNullOrEmpty(student.Name) || string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.Password))
                {
                    throw new InvalidOperationException("Datele de antrenament conțin valori nule sau incorecte.");
                }

                if (student.AverageGrade < 0 || student.AverageGrade > 10)
                {
                    throw new InvalidOperationException("AverageGrade trebuie să fie între 0 și 10.");
                }

                if (student.PercentageCompletedCourses < 0 || student.PercentageCompletedCourses > 100)
                {
                    throw new InvalidOperationException("PercentageCompletedCourses trebuie să fie între 0 și 100.");
                }
            }

            foreach (var student in sampleData)
            {
                Console.WriteLine($"Name: {student.Name}, Email: {student.Email}, Password: {student.Password}, CreatedAt: {student.CreatedAt}, LastLogin: {student.LastLogin}, AverageGrade: {student.AverageGrade}, PercentageCompletedCourses: {student.PercentageCompletedCourses}, LearningPath: {student.LearningPath}");
            }

            Console.WriteLine("Datele de antrenament sunt variate și corect populate.");
           // studentPerformancePredictionModel.TrainAverageGradeModel(sampleData);
            //studentPerformancePredictionModel.TrainPercentageCompletedCoursesModel(sampleData);
            //studentPerformancePredictionModel.TrainLearningPathModel(sampleData);
        }

        //[HttpPost("predictAverageGrade")]
        //public ActionResult<float> PredictAverageGrade(StudentData student)
        //{
        //    return studentPerformancePredictionModel.PredictAverageGrade(student);
        //}

        //[HttpPost("predictPercentageCompletedCourses")]
        //public ActionResult<float> PredictPercentageCompletedCourses(StudentData student)
        //{
        //    return studentPerformancePredictionModel.PredictPercentageCompletedCourses(student);
        //}

        //[HttpPost("predictLearningPath")]
        //public ActionResult<string> PredictLearningPath(StudentData student)
        //{
        //    return studentPerformancePredictionModel.PredictLearningPath(student);
        //}

        //[HttpPost("predictAll")]
        //public ActionResult<StudentPredictions> PredictAll(StudentData student)
        //{
        //    var predictions = new StudentPredictions
        //    {
        //        AverageGrade = studentPerformancePredictionModel.PredictAverageGrade(student),
        //        PercentageCompletedCourses = studentPerformancePredictionModel.PredictPercentageCompletedCourses(student),
        //        LearningPath = studentPerformancePredictionModel.PredictLearningPath(student) 
        //    };
        //    return predictions;
        //}


    }
}
