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
    class ExerciseGetDtoTests : BaseTest
    {
        [Test]
        public void Validate_EverythingWorks_ReturnsNoErrorMessage()
        {
            // Arrange
            var request = new ExerciseGetDto { Id = NextId, Name = $"{NextId}", UserId = NextId };

            // Act
            var validator = new ExerciseGetDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_ExerciseIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseGetDto { Id = NextId };

            // Act
            var validator = new ExerciseGetDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Exercise' does not exist."
                && x.PropertyName == nameof(ExerciseGetDto.Id));

            Assert.IsTrue(hasCorrectErrrorMessage);
        }

        [Test]
        public void Validate_UnableToGetExerciseYouDoNotOwn_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var user1 = new User { Id = NextId };
            var user2 = new User { Id = NextId };

            Context.Users.Add(user1);
            Context.Users.Add(user2);

            var exercise = new Exercise { Id = NextId, UserId = user1.Id };
            Context.Exercises.Add(exercise);
            Context.SaveChanges();

            var request = new ExerciseGetDto { UserId = user2.Id, Id = exercise.Id };

            // Act
            var validator = new ExerciseGetDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "Unable to get an 'Exercise' you do not own."
                && x.PropertyName == "");

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseGetDto();

            // Act
            var validator = new ExerciseGetDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(ExerciseGetDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseGetDto { UserId = NextId };

            //Act
            var validator = new ExerciseGetDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User' does not exist."
                && x.PropertyName == nameof(ExerciseGetDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_ExerciseIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseGetDto();

            // Act
            var validator = new ExerciseGetDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == nameof(ExerciseGetDto.Id));

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
}
