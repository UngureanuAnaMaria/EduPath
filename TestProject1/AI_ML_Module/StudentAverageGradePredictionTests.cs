using Application.AI_ML_Module;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1.AI_ML_Module
{
    [TestClass]
    public class StudentAverageGradePredictionTests
    {
        [TestMethod]
        public void StudentAverageGradePrediction_SetAverageGrade_ShouldHoldCorrectValue()
        {
            // Arrange
            var prediction = new StudentAverageGradePrediction();
            float expectedAverageGrade = 9.5f;

            // Act
            prediction.AverageGrade = expectedAverageGrade;

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(expectedAverageGrade, prediction.AverageGrade,
                "The AverageGrade property did not hold the correct value.");
        }

        [TestMethod]
        public void StudentAverageGradePrediction_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var prediction = new StudentAverageGradePrediction();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsNotNull(prediction,
                "The StudentAverageGradePrediction object was not initialized.");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(0.0f, prediction.AverageGrade,
                "The AverageGrade property should default to 0.0f.");
        }
    }
}
