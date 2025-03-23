using System;
using System.Collections.Generic;
using System.Linq;
using Application.AI_ML_Module;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1.AI_ML_Module
{
    [TestClass]
    public class StudentDataGeneratorTests
    {
        [TestMethod]
        public void GenerateSampleData_ShouldReturnListOfStudentsWithinValidCountRange()
        {
            // Arrange
            int minStudents = 50;
            int maxStudents = 100;

            // Act
            List<StudentData> students = StudentDataGenerator.GenerateSampleData();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(students, "The student list should not be null.");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                students.Count >= minStudents && students.Count <= maxStudents,
                $"The number of students ({students.Count}) should be between {minStudents} and {maxStudents}."
            );
        }

        [TestMethod]
        public void GenerateSampleData_ShouldGenerateStudentsWithNonNullProperties()
        {
            // Act
            List<StudentData> students = StudentDataGenerator.GenerateSampleData();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(students.Count > 0, "The student list should not be empty.");

            foreach (var student in students)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(string.IsNullOrEmpty(student.Name), "Student name should not be null or empty.");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(string.IsNullOrEmpty(student.Email), "Student email should not be null or empty.");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(string.IsNullOrEmpty(student.Password), "Student password should not be null or empty.");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(student.CreatedAt, "CreatedAt should not be null.");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(student.LastLogin, "LastLogin should not be null.");
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(student.LearningPath, "LearningPath should not be null.");
            }
        }

        [TestMethod]
        public void GenerateSampleData_ShouldGenerateAverageGradeWithinValidRange()
        {
            // Arrange
            float minGrade = 8.0f;
            float maxGrade = 10.0f;

            // Act
            List<StudentData> students = StudentDataGenerator.GenerateSampleData();

            // Assert
            foreach (var student in students)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                    student.AverageGrade >= minGrade && student.AverageGrade <= maxGrade,
                    $"Student average grade ({student.AverageGrade}) should be between {minGrade} and {maxGrade}."
                );
            }
        }

        [TestMethod]
        public void GenerateSampleData_ShouldGeneratePercentageCompletedCoursesWithinValidRange()
        {
            // Arrange
            float minPercentage = 90.0f;
            float maxPercentage = 100.0f;

            // Act
            List<StudentData> students = StudentDataGenerator.GenerateSampleData();

            // Assert
            foreach (var student in students)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                    student.PercentageCompletedCourses >= minPercentage && student.PercentageCompletedCourses <= maxPercentage,
                    $"Student percentage completed courses ({student.PercentageCompletedCourses}) should be between {minPercentage} and {maxPercentage}."
                );
            }
        }

        [TestMethod]
        public void GenerateSampleData_ShouldRandomlyGenerateLearningPaths()
        {
            // Arrange
            var validLearningPaths = new List<string>
            {
                "Data Science", "Web Development", "Mobile Development", "Cloud Computing", "Cyber Security"
            };

            // Act
            List<StudentData> students = StudentDataGenerator.GenerateSampleData();
            var generatedPaths = students.Select(s => s.LearningPath).Distinct();

            // Assert
            foreach (var path in generatedPaths)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                    validLearningPaths.Contains(path),
                    $"Learning path '{path}' is not a valid path."
                );
            }
        }
    }
}
