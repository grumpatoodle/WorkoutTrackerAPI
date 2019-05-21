using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using StarterProject.Api.Features.Exercises.Dtos;
using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Users;

namespace StarterProject.Api.Tests.Features.Exercises.Dtos
{
    [TestFixture]
    class ExerciseEditDtoTests : BaseTest
    {
        [Test]
        public void Validate_EverythingIsWorking_ReturnsNoErrorMessage()
        {
            // Arrange
            var request = new ExerciseEditDto { UserId = NextId, Id = NextId, Name = $"{NextId}" };

            // Act
            var validator = new ExerciseEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Name' must not be empty."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_ExerciseNameIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseEditDto();

            // Act
            var validator = new ExerciseEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Name' must not be empty."
                && x.PropertyName == nameof(ExerciseEditDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseEditDto();

            // Act
            var validator = new ExerciseEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(ExerciseEditDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoMuscleGroupIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new ExerciseEditDto();

            // Act
            var validator = new ExerciseEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Muscle Group' must not be empty."
                && x.PropertyName == nameof(ExerciseEditDto.MuscleGroup));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoNameAlreadyExists_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var name = "Squat";
            var user = new User { Id = NextId };
            var muscleGroup = "Quadriceps";

            Context.Users.Add(user);

            var exercise1 = new Exercise { Id = NextId, Name = name, UserId = user.Id, MuscleGroup = muscleGroup };
            var exercise2 = new Exercise { Id = NextId, Name = $"{NextId}", UserId = user.Id, MuscleGroup = muscleGroup };

            Context.Exercises.Add(exercise1);
            Context.Exercises.Add(exercise2);
            Context.SaveChanges();

            var request = new ExerciseEditDto { UserId = user.Id, Name = name, Id = exercise2.Id, MuscleGroup = muscleGroup };

            // Act
            var validator = new ExerciseEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == $"You already have an exercise of '{name}'"
                && x.PropertyName == nameof(ExerciseEditDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UnableToEditExerciseYouDoNotOwn_ReturnsCorrectErrorMessage()
        {
            // Assert
            var user1 = new User { Id = NextId };
            var user2 = new User { Id = NextId };

            Context.Users.Add(user1);
            Context.Users.Add(user2);

            var exercise = new Exercise { Id = NextId, UserId = user1.Id };
            Context.Exercises.Add(exercise);
            Context.SaveChanges();

            var request = new ExerciseEditDto { Id = exercise.Id, UserId = user2.Id, };

            // Act
            var validator = new ExerciseEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "You cannot edit an 'Exercise' you do not own."
                && x.PropertyName == "");

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
 }
