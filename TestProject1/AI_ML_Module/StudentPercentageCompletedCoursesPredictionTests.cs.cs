using System;
using Application.AI_ML_Module;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1.AI_ML_Module
{
    [TestClass]
    public class StudentPercentageCompletedCoursesPredictionTests
    {
        [TestMethod]
        public void PercentageCompletedCoursesPrediction_ShouldNotBeNull()
        {
            // Arrange
            var prediction = new StudentPercentageCompletedCoursesPrediction();

            // Act
            prediction.PercentageCompletedCourses = 85.5f;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(prediction.PercentageCompletedCourses, "PercentageCompletedCourses should not be null.");
        }

        [TestMethod]
        public void PercentageCompletedCoursesPrediction_ShouldBeInValidRange()
        {
            // Arrange
            var prediction = new StudentPercentageCompletedCoursesPrediction();

            // Act
            prediction.PercentageCompletedCourses = 80.0f;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                prediction.PercentageCompletedCourses >= 0.0f && prediction.PercentageCompletedCourses <= 100.0f,
                "PercentageCompletedCourses should be between 0 and 100."
            );
        }

        [TestMethod]
        public void PercentageCompletedCoursesPrediction_ShouldReturnCorrectValue()
        {
            // Arrange
            var prediction = new StudentPercentageCompletedCoursesPrediction
            {
                PercentageCompletedCourses = 92.5f
            };

            // Act
            float predictedPercentage = prediction.PercentageCompletedCourses;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(92.5f, predictedPercentage, "Percentage completed courses prediction is incorrect.");
        }

    }
}
