using NUnit.Framework;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Eqiupments.Dtos;
using StarterProject.Api.Features.Equipments.Dtos;
using StarterProject.Api.Features.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StarterProject.Api.Tests.Features.Equipments.Dtos
{
    public class EquipmentDeleteDtoTests : BaseTest
    {
        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentDeleteDto();

            var validator = new EquipmentDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(EquipmentDeleteDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentDeleteDto { UserId = NextId };

            var validator = new EquipmentDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == nameof(EquipmentDeleteDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EquipmentIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentDeleteDto();

            var validator = new EquipmentDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == nameof(EquipmentDeleteDto.Id));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EquipmentIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentDeleteDto { Id = NextId };

            var validator = new EquipmentDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "This 'Equipment' does not exist."
                && x.PropertyName == nameof(EquipmentDeleteDto.Id));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UnableToAccessEquipmentNotOwned_ReturnsCorrectErrorMessage()
        {
            var user1 = new User { Id = NextId };
            var user2 = new User { Id = NextId };

            Context.Users.Add(user1);
            Context.Users.Add(user2);

            var equipment = new Equipment { Id = NextId, UserId = user1.Id };
            Context.Equipments.Add(equipment);

            Context.SaveChanges();

            var equipmentDeleteDto = new EquipmentDeleteDto { Id = equipment.Id, UserId = user2.Id };

            var validator = new EquipmentDeleteDtoValidator(Context);

            var result = validator.Validate(equipmentDeleteDto);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "Unable to delete an 'Equipment' that you do not own."
                && x.PropertyName == "");

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EverythingWorks_ReturnsNoErrorMessage()
        {
            var request = new EquipmentDeleteDto { UserId = NextId, Id = NextId };

            var validator = new EquipmentDeleteDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Id' must not be empty."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrrorMessage);
        }
    }
}
