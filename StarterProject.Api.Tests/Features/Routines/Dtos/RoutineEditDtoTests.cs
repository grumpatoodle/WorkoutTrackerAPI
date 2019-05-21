using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using StarterProject.Api.Features.Routines.Dtos;
using StarterProject.Api.Data;
using StarterProject.Api.Features.Users;
using StarterProject.Api.Data.Entites;

namespace StarterProject.Api.Tests.Features.Routines.Dtos
{
    [TestFixture]
    class RoutineEditDtoTests : BaseTest
    {
        [Test]
        public void Validate_EverythingIsWorking_ReturnsNoErrorMessage()
        {
            // Arrange
            var request = new RoutineEditDto { UserId = NextId, Id = NextId, Name = $"{NextId}"};

            // Act
            var validator = new RoutineEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "Any Error Message."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_RoutineNameIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineEditDto();

            // Act
            var validator = new RoutineEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x => 
                x.ErrorMessage == "'Name' must not be empty."
                && x.PropertyName == nameof(RoutineEditDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }
         
        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineEditDto();

            // Act
            var validator = new RoutineEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(RoutineEditDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoNameAlreadyExists_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var name = "leg day";
            var user = new User { Id = NextId };

            Context.Users.Add(user);

            var routine1 = new Routine { Id = NextId, Name = name, UserId = user.Id };
            var routine2 = new Routine { Id = NextId, Name = $"{NextId}" , UserId = user.Id};

            Context.Routines.Add(routine1);
            Context.Routines.Add(routine2);
            Context.SaveChanges();

            var request = new RoutineEditDto { UserId = user.Id, Name = name, Id = routine2.Id };

            // Act
            var validator = new RoutineEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == $"You already have a routine of '{name}'"
                && x.PropertyName == nameof(RoutineEditDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UnableToEditRoutineYouDoNotOwn_ReturnsCorrectErrorMessage()
        {
            // Assert
            var user1 = new User { Id = NextId };
            var user2 = new User { Id = NextId };

            Context.Users.Add(user1);
            Context.Users.Add(user2);

            var routine = new Routine { Id = NextId, UserId = user1.Id };
            Context.Routines.Add(routine);
            Context.SaveChanges();

            var request = new RoutineEditDto { Id = routine.Id, UserId = user2.Id, };

            // Act
            var validator = new RoutineEditDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "You cannot edit a 'routine' you do not own."
                && x.PropertyName == "");

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
}
