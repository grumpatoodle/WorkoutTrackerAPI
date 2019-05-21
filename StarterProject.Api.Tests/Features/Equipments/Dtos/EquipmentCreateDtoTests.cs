using NUnit.Framework;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Equipments.Dtos;
using StarterProject.Api.Features.Users;
using System.Linq;

namespace StarterProject.Api.Tests.Features.Equipments.Dtos
{
    [TestFixture]
    public class EquipmentCreateDtoTests : BaseTest
    {
        [Test]
        public void Validate_UserIdIsEmpty_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentCreateDto();

            var validator = new EquipmentCreateDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'User Id' must not be empty."
                && x.PropertyName == nameof(EquipmentCreateDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_UserIdDoesNotExist_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentCreateDto { UserId = NextId };

            var validator = new EquipmentCreateDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == nameof(EquipmentCreateDto.UserId));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EquipmentNameIsEmpty_ReturnsCorrectErrorMessage()
        {
            var request = new EquipmentCreateDto();

            var validator = new EquipmentCreateDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "'Name' must not be empty."
                && x.PropertyName == nameof(EquipmentCreateDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_DtoNameAlreadyExists_ReturnsCorrectErrorMessage()
        {
            var name = "Barbell";
            var user = new User { Id = NextId };
            Context.Users.Add(user);

            var equipment = new Equipment { UserId = user.Id, Name = name };
            Context.Equipments.Add(equipment);
            Context.SaveChanges();

            var request = new EquipmentCreateDto { UserId = user.Id, Name = name };
            var validator = new EquipmentCreateDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == $"You already have the equipment '{request.Name}'"
                && x.PropertyName == nameof(EquipmentCreateDto.Name));

            Assert.IsTrue(hasCorrectErrorMessage);
        }

        [Test]
        public void Validate_EverythingWorks_ReturnsNoErrorMessage()
        {
            var name = "Barbell";
            var request = new EquipmentCreateDto { UserId = NextId, Name = name };

            var validator = new EquipmentCreateDtoValidator(Context);
            var result = validator.Validate(request);

            var hasCorrectErrorMessage = result.Errors.Any(x =>
                x.ErrorMessage == "The 'User' does not exist."
                && x.PropertyName == "");

            Assert.IsTrue(!hasCorrectErrorMessage);
        }
    }
}
