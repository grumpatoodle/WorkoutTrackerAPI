using NUnit.Framework;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Equipments;
using StarterProject.Api.Features.Equipments.Dtos;
using StarterProject.Api.Features.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterProject.Api.Tests.Features.Equipments
{
    public class EquipmentRepositoryTests : BaseTest
    {
        [Test]
        public void CreateEquipment_NameHasValue_ReturnsNameThatWasGiven()
        {
            var name = $"{NextId}";
            var request = new EquipmentCreateDto { Name = name };

            var repo = new EquipmentRepository(Context);
            var result = repo.CreateEquipment(request);

            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void CreateEquipment_NameHasValue_ContextHasNewEquipmentWithCorrectName()
        {
            var name = $"{NextId}";
            var request = new EquipmentCreateDto { Name = name };

            var repo = new EquipmentRepository(Context);
            var result = repo.CreateEquipment(request);
            var equipmentFromContext = Context.Equipments.Find(result.Id);

            Assert.AreEqual(name, equipmentFromContext.Name);
        }
    }
}
