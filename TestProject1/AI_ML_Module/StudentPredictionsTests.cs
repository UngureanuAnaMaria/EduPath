using Application.AI_ML_Module;
using Xunit;

namespace Application.AI_ML_Module.Tests
{
    public class StudentPredictionsTests
    {
        [Fact]
        public void Should_Set_AverageGrade_And_Retrieve_Correctly()
        {
            // Arrange
            var studentPrediction = new StudentPredictions();
            float expectedAverageGrade = 85.5f;

            // Act
            studentPrediction.AverageGrade = expectedAverageGrade;

            // Assert
            Assert.Equal(expectedAverageGrade, studentPrediction.AverageGrade);
        }

        [Fact]
        public void Should_Set_PercentageCompletedCourses_And_Retrieve_Correctly()
        {
            // Arrange
            var studentPrediction = new StudentPredictions();
            float expectedPercentageCompletedCourses = 92.5f;

            // Act
            studentPrediction.PercentageCompletedCourses = expectedPercentageCompletedCourses;

            // Assert
            Assert.Equal(expectedPercentageCompletedCourses, studentPrediction.PercentageCompletedCourses);
        }

        [Fact]
        public void Should_Set_LearningPath_And_Retrieve_Correctly()
        {
            // Arrange
            var studentPrediction = new StudentPredictions();
            string expectedLearningPath = "Data Science";

            // Act
            studentPrediction.LearningPath = expectedLearningPath;

            // Assert
            Assert.Equal(expectedLearningPath, studentPrediction.LearningPath);
        }

        [Fact]
        public void Should_Set_All_Properties_And_Retrieve_Correctly()
        {
            // Arrange
            var studentPrediction = new StudentPredictions
            {
                AverageGrade = 88.0f,
                PercentageCompletedCourses = 95.0f,
                LearningPath = "Machine Learning"
            };

            // Act & Assert
            Assert.Equal(88.0f, studentPrediction.AverageGrade);
            Assert.Equal(95.0f, studentPrediction.PercentageCompletedCourses);
            Assert.Equal("Machine Learning", studentPrediction.LearningPath);
        }
    }
}
