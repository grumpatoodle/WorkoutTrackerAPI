using NUnit.Framework;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Routines;
using StarterProject.Api.Features.Routines.Dtos;
using StarterProject.Api.Features.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterProject.Api.Tests.Features.Routines
{
    [TestFixture]
    class RoutineRepositoryTests : BaseTest
    {
        [Test]
        public void CreateRoutine_NameHasValue_ReturnsNameThatWasGiven()
        {
            // Arrange
            var name = $"{NextId}";
            var request = new RoutineCreateDto { Name = name };

            // Act
            var repo = new RoutineRepository(Context);
            var result = repo.CreateRoutine(request);

            // Assert
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void CreateRoutine_NameHasValue_ContextHasNewRoutineWithCorrectName()
        {
            // Arrange
            var name = $"{NextId}";
            var request = new RoutineCreateDto { Name = name };

            // Act

            var repo = new RoutineRepository(Context);
            var result = repo.CreateRoutine(request);
            var routineFromContext = Context.Routines.Find(result.Id);

            // Assert
            Assert.AreEqual(name, routineFromContext.Name);
        }
    }
}
