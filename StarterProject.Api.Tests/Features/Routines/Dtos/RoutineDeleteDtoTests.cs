using NUnit.Framework;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Routines.Dtos;
using StarterProject.Api.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarterProject.Api.Tests.Features.Routines.Dtos
{
    [TestFixture]
    public class RoutineDeleteDtoTests : BaseTest
    {
        [Test]
        public void Validate_EverythingWorks_ReturnsNoErrorMessage()
        {
            // Arrange
            var request = new RoutineDeleteDto { UserId = NextId, Id = NextId};

            // Act
            var validator = new RoutineDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrrorMessage);
        }

        [Test]
        public void Validate_RoutineIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineDeleteDto();

            // Act
            var validator = new RoutineDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == nameof(RoutineDeleteDto.Id));

            Assert.IsTrue(hasCorrectErrrorMessage);
        }

        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineDeleteDto();

            // Act
            var validator = new RoutineDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(RoutineDeleteDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_RoutineIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineDeleteDto { Id = NextId };

            // Act
            var validator = new RoutineDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Routine' does not exist."
                && x.PropertyName == nameof(RoutineDeleteDto.Id));

            Assert.IsTrue(hasCorrectErrrorMessage);
        }

        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineDeleteDto { UserId = NextId };

            //Act
            var validator = new RoutineDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User' does not exist."
                && x.PropertyName == nameof(RoutineDeleteDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UnableToDeleteRoutineYouDoNotOwn_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var user1 = new User { Id = NextId };
            var user2 = new User { Id = NextId };

            Context.Users.Add(user1);
            Context.Users.Add(user2);

            var routine = new Routine { Id = NextId, UserId = user1.Id };

            Context.Routines.Add(routine);
            Context.SaveChanges();

            var request = new RoutineDeleteDto { UserId = user2.Id, Id = routine.Id };

            // Act
            var validator = new RoutineDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "Unable to delete a 'Routine' you do not own."
                && x.PropertyName == "");

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
}
