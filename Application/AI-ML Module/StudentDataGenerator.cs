//using System;
//using System.Collections.Generic;

//namespace Application.AI_ML_Module_Generator
//{
//    public class StudentDataGenerator
//    {
//        public static List<StudentData> GenerateSampleData()
//        {
//            var random = new Random();
//            var students = new List<StudentData>();
//            int numberOfStudents = random.Next(50, 101);

//            for (int i = 0; i < numberOfStudents; i++)
//            {
//                students.Add(new StudentData
//                {
//                    Name = $"Student {i + 1}",
//                    Email = $"student{i + 1}@example.com",
//                    Password = $"password {i + 1}",
//                    Status = random.Next(0, 2) == 1,
//                    CreatedAt = DateTime.Now.AddMonths(-random.Next(1, 24)),
//                    LastLogin =  DateTime.Now.AddDays(-random.Next(1, 30)),
//                    AverageGrade = 8.0f + (float)(random.NextDouble() * 2.0f),
//                    PercentageCompletedCourses = 90.0f + (float)(random.NextDouble() * 10.0f)
//                });
//            }

//            return students;
//        }
//    }
//}

using System;
using System.Collections.Generic;

namespace Application.AI_ML_Module
{
    public class StudentDataGenerator
    {
        public static List<StudentData> GenerateSampleData()
        {
            var random = new Random();
            var students = new List<StudentData>();
            int numberOfStudents = random.Next(50, 101);
            var learningPaths = new List<string> { "Data Science", "Web Development", "Mobile Development", "Cloud Computing", "Cyber Security" };

            for (int i = 0; i < numberOfStudents; i++)
            {
                students.Add(new StudentData
                {
                    Name = $"Student {i + 1}",
                    Email = $"student{i + 1}@example.com",
                    Password = $"password {i + 1}",
                    Status = random.Next(0, 2) == 1,
                    CreatedAt = DateTime.Now.AddMonths(-random.Next(1, 24)),
                    LastLogin = DateTime.Now.AddDays(-random.Next(1, 30)),
                    AverageGrade = 8.0f + (float)(random.NextDouble() * 2.0f),
                    PercentageCompletedCourses = 90.0f + (float)(random.NextDouble() * 10.0f),
                    LearningPath = learningPaths[random.Next(learningPaths.Count)]
                });
            }

            return students;
        }
    }
}


