using System;
using Application.AI_ML_Module;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1.AI_ML_Module
{
    [TestClass]
    public class StudentLearningPathPredictionTests
    {
        [TestMethod]
        public void LearningPathPrediction_ShouldNotBeNull()
        {
            // Arrange
            var prediction = new StudentLearningPathPrediction();

            // Act
            prediction.LearningPath = "Data Science";

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(prediction.LearningPath, "LearningPath should not be null.");
        }

        [TestMethod]
        public void LearningPathPrediction_ShouldContainValidLearningPath()
        {
            // Arrange
            var validLearningPaths = new List<string>
            {
                "Data Science", "Web Development", "Mobile Development", "Cloud Computing", "Cyber Security"
            };
            var prediction = new StudentLearningPathPrediction();

            // Act
            prediction.LearningPath = "Web Development";

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(
                validLearningPaths.Contains(prediction.LearningPath),
                $"Learning path '{prediction.LearningPath}' is not a valid path."
            );
        }

        [TestMethod]
        public void LearningPathPrediction_ShouldNotBeEmpty()
        {
            // Arrange
            var prediction = new StudentLearningPathPrediction();

            // Act
            prediction.LearningPath = "Mobile Development";

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(
                string.IsNullOrEmpty(prediction.LearningPath),
                "LearningPath should not be empty."
            );
        }

        [TestMethod]
        public void LearningPathPrediction_ShouldReturnCorrectLearningPath()
        {
            // Arrange
            var prediction = new StudentLearningPathPrediction
            {
                LearningPath = "Cloud Computing"
            };

            // Act
            string predictedLearningPath = prediction.LearningPath;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Cloud Computing", predictedLearningPath, "Learning path prediction is incorrect.");
        }
    }
}
