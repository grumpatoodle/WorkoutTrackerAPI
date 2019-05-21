using NUnit.Framework;
using StarterProject.Api.Features.Exercises.Dtos;
using System.Linq;
using StarterProject.Api.Features.Users;
using FluentValidation;
using StarterProject.Api.Data.Entites;

namespace StarterProject.Api.Tests.Features.Exercises.Dtos
{
    [TestFixture]
    public class ExerciseCreateDtoTests : BaseTest
    {
        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseCreateDto { UserId = NextId };

            // Act
            var validator = new ExerciseCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == nameof(ExerciseCreateDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EverythingWorks_ReturnsNoErrorMessage()
        {
            // Arrange
            var name = "Squat";
            var request = new ExerciseCreateDto { UserId = NextId, Name = name };

            // Act
            var validator = new ExerciseCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoNameIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseCreateDto();

            // Act
            var validator = new ExerciseCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Name' must not be empty."
                && x.PropertyName == nameof(ExerciseCreateDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoMuscleGroupIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseCreateDto();

            // Act
            var validator = new ExerciseCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Muscle Group' must not be empty."
                && x.PropertyName == nameof(ExerciseCreateDto.MuscleGroup));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoNameAlreadyExists_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var name = "Squat";
            var user = new User { Id = NextId };
            Context.Users.Add(user);

            var exercise = new Exercise { UserId = user.Id, Name = name };
            Context.Exercises.Add(exercise);
            Context.SaveChanges();

            var request = new ExerciseCreateDto { UserId = user.Id, Name = name };

            // Act
            var validator = new ExerciseCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == $"You already have an exercise of '{request.Name}'"
                && x.PropertyName == nameof(ExerciseCreateDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdIsNotEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseCreateDto();

            // Act
            var validator = new ExerciseCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(ExerciseCreateDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
}
