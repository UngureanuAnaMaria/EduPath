using Domain.Predictions;
using Xunit;
using FluentAssertions;

namespace Domain.Tests
{
    public class StudentPredictionDatasTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValuesAreSet()
        {
            // Arrange
            var studentPredictionData = new StudentPredictionDatas
            {
                Name = "John Doe",
                Gender = "Male",
                Age = 22f,
                GPA = 3.8f,
                Major = "Computer Science",
                InterestedDomain = "Data Science",
                Projects = "AI Research",
                Python = "Advanced",
                SQL = "Intermediate",
                Java = "Intermediate"
            };

            // Act & Assert
            studentPredictionData.Name.Should().Be("John Doe");
            studentPredictionData.Gender.Should().Be("Male");
            studentPredictionData.Age.Should().Be(22f);
            studentPredictionData.GPA.Should().Be(3.8f);
            studentPredictionData.Major.Should().Be("Computer Science");
            studentPredictionData.InterestedDomain.Should().Be("Data Science");
            studentPredictionData.Projects.Should().Be("AI Research");
            studentPredictionData.Python.Should().Be("Advanced");
            studentPredictionData.SQL.Should().Be("Intermediate");
            studentPredictionData.Java.Should().Be("Intermediate");
        }

        [Fact]
        public void Constructor_ShouldAllowNullValues_ForStringProperties()
        {
            // Arrange
            var studentPredictionData = new StudentPredictionDatas
            {
                Name = null,
                Gender = null,
                Major = null,
                InterestedDomain = null,
                Projects = null,
                Python = null,
                SQL = null,
                Java = null
            };

            // Act & Assert
            studentPredictionData.Name.Should().BeNull();
            studentPredictionData.Gender.Should().BeNull();
            studentPredictionData.Major.Should().BeNull();
            studentPredictionData.InterestedDomain.Should().BeNull();
            studentPredictionData.Projects.Should().BeNull();
            studentPredictionData.Python.Should().BeNull();
            studentPredictionData.SQL.Should().BeNull();
            studentPredictionData.Java.Should().BeNull();
        }

        [Fact]
        public void Constructor_ShouldAllowDefaultZeroValues_ForNumericProperties()
        {
            // Arrange
            var studentPredictionData = new StudentPredictionDatas();

            // Act & Assert
            studentPredictionData.Age.Should().Be(0f);
            studentPredictionData.GPA.Should().Be(0f);
        }

        [Theory]
        [InlineData(18f)]
        [InlineData(22f)]
        [InlineData(30f)]
        public void Age_ShouldBeGreaterThanZero(float age)
        {
            // Arrange
            var studentPredictionData = new StudentPredictionDatas
            {
                Age = age
            };

            // Act & Assert
            studentPredictionData.Age.Should().BeGreaterThan(0f);
        }

        [Fact]
        public void Should_AllowValidPropertyChanges()
        {
            // Arrange
            var studentPredictionData = new StudentPredictionDatas
            {
                Name = "Alice",
                Gender = "Female",
                Age = 21f,
                GPA = 3.9f,
                Major = "Engineering",
                InterestedDomain = "Robotics",
                Projects = "Robotics Research",
                Python = "Intermediate",
                SQL = "Beginner",
                Java = "Advanced"
            };

            // Act
            studentPredictionData.Name = "Bob";
            studentPredictionData.Gender = "Male";
            studentPredictionData.Age = 25f;
            studentPredictionData.GPA = 3.7f;
            studentPredictionData.Major = "Mathematics";
            studentPredictionData.InterestedDomain = "AI";
            studentPredictionData.Projects = "AI Research";
            studentPredictionData.Python = "Advanced";
            studentPredictionData.SQL = "Intermediate";
            studentPredictionData.Java = "Beginner";

            // Assert
            studentPredictionData.Name.Should().Be("Bob");
            studentPredictionData.Gender.Should().Be("Male");
            studentPredictionData.Age.Should().Be(25f);
            studentPredictionData.GPA.Should().Be(3.7f);
            studentPredictionData.Major.Should().Be("Mathematics");
            studentPredictionData.InterestedDomain.Should().Be("AI");
            studentPredictionData.Projects.Should().Be("AI Research");
            studentPredictionData.Python.Should().Be("Advanced");
            studentPredictionData.SQL.Should().Be("Intermediate");
            studentPredictionData.Java.Should().Be("Beginner");
        }
    }
}
