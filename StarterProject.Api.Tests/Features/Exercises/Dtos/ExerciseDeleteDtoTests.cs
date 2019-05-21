using NUnit.Framework;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Exercises.Dtos;
using StarterProject.Api.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarterProject.Api.Tests.Features.Exercises.Dtos
{
    [TestFixture]
    class ExerciseDeleteDtoTests : BaseTest
    {
        [Test]
        public void Validate_EverythingWorks_ReturnsNoErrorMessage()
        {
            // Arrange
            var request = new ExerciseDeleteDto { UserId = NextId, Id = NextId };

            // Act
            var validator = new ExerciseDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_ExerciseIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseDeleteDto();

            // Act
            var validator = new ExerciseDeleteDtoValidator(Context);
            var result = validator.Validate(request);
             
            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == nameof(ExerciseDeleteDto.Id));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_ExerciseIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseDeleteDto { Id = NextId };

            // Act
            var validator = new ExerciseDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
               x.ErrorMessage == "'Exercise' does not exist."
               && x.PropertyName == nameof(ExerciseDeleteDto.Id));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseDeleteDto();

            // Act
            var validator = new ExerciseDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(ExerciseDeleteDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseDeleteDto { UserId = NextId};

            // Act
            var validator = new ExerciseDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == nameof(ExerciseDeleteDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UnableToAccessExerciseNotOwned_ReturnsCorrectErrorMessage()
        {
            var user1 = new User { Id = NextId };
            var user2 = new User { Id = NextId };

            Context.Users.Add(user1);
            Context.Users.Add(user2);

            var exercise = new Exercise { Id = NextId, UserId = user1.Id };
            Context.Exercises.Add(exercise);

            Context.SaveChanges();

            var exerciseDeleteDto = new ExerciseDeleteDto { Id = exercise.Id, UserId = user2.Id };

            var validator = new ExerciseDeleteDtoValidator(Context);

            var result = validator.Validate(exerciseDeleteDto);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "You cannot delete an 'Exercise' you do not own."
                && x.PropertyName == "");

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
}
