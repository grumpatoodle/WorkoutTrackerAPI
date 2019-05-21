using NUnit.Framework;
using StarterProject.Api.Features.Exercises;
using StarterProject.Api.Features.Exercises.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace StarterProject.Api.Tests.Features.Exercises
{
    [TestFixture]
    class ExerciseRepositoryTests : BaseTest
    {
        [Test]
        public void CreateExercise_NameHasValue_ReturnsNameThatWasGiven()
        {
            // Arrange
            var name = $"{NextId}";
            var request = new ExerciseCreateDto { Name = name };

            // Act
            var repo = new ExerciseRepository(Context);
            var result = repo.CreateExercise(request);

            // Assert
            Assert.AreEqual(name, result.Name);
        }

        [Test]
        public void CreateExercise_NameHasValue_ContextHasNewExerciseWithCorrectName()
        {
            // Arrange
            var name = $"{NextId}";
            var request = new ExerciseCreateDto { Name = name };

            // Act

            var repo = new ExerciseRepository(Context);
            var result = repo.CreateExercise(request);
            var exerciseFromContext = Context.Exercises.Find(result.Id);

            // Assert
            Assert.AreEqual(name, exerciseFromContext.Name);
        }
    }
}
