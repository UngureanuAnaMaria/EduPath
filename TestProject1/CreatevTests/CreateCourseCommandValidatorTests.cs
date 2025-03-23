using FluentValidation.TestHelper;
using Application.Use_Cases.Commands.Create;
using Xunit;

namespace Application.Tests
{
    public class CreateCourseCommandValidatorTests
    {
        private readonly CreateCourseCommandValidator _validator;

        public CreateCourseCommandValidatorTests()
        {
            _validator = new CreateCourseCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Empty()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Name = string.Empty, // Empty name
                Description = "Valid description"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Course name is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Name_Exceeds_Max_Length()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Name = new string('A', 201), // Name exceeding 200 characters
                Description = "Valid description"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                .WithErrorMessage("Course name must not exceed 200 characters.");
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Name = "Valid Course Name",
                Description = string.Empty // Empty description
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Course description is required.");
        }

        [Fact]
        public void Should_Have_Error_When_Description_Exceeds_Max_Length()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Name = "Valid Course Name",
                Description = new string('A', 501) // Description exceeding 500 characters
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Description)
                .WithErrorMessage("Course description must not exceed 500 characters.");
        }

        [Fact]
        public void Should_Not_Have_Any_Validation_Error_When_Inputs_Are_Valid()
        {
            // Arrange
            var command = new CreateCourseCommand
            {
                Name = "Valid Course Name",
                Description = "Valid description"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
