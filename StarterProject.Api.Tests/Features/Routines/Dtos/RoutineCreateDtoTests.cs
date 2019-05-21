using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;
using StarterProject.Api.Features.Routines.Dtos;
using StarterProject.Api.Features.Users.Dtos;
using StarterProject.Api.Data;
using StarterProject.Api.Features.Users;
using StarterProject.Api.Data.Entites;

namespace StarterProject.Api.Tests.Features.Routines.Dtos
{
    [TestFixture]
    class RoutineCreateDtoTests : BaseTest
    {
        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineCreateDto { UserId = NextId };

            // Act
            var validator = new RoutineCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == nameof(RoutineCreateDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EVerythingWorks_ReturnsNoErrorMessage()
        {
            // Arrange
            var name = "Leg Day";
            var request = new RoutineCreateDto { UserId = NextId, Name = name };

            // Act
            var validator = new RoutineCreateDtoValidator(Context);
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
            var request = new RoutineCreateDto();

            // Act
            var validator = new RoutineCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Name' must not be empty."
                && x.PropertyName == nameof(RoutineCreateDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoNameAlreadyExists_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var name = "leg day";
            var user = new User { Id = NextId };
            Context.Users.Add(user);

            var routine = new Routine { UserId = user.Id, Name = name };
            Context.Routines.Add(routine);
            Context.SaveChanges();

            var request = new RoutineCreateDto { UserId = user.Id, Name = name };

            // Act
            var validator = new RoutineCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == $"You already have a routine of '{request.Name}'"
                && x.PropertyName == nameof(RoutineCreateDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdIsNotEmpty_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var request = new RoutineCreateDto();

            // Act
            var validator = new RoutineCreateDtoValidator(Context);
            var result = validator.Validate(request);

            // Assert
            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(RoutineCreateDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }
    }
}
