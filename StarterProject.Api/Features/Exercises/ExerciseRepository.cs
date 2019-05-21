using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using StarterProject.Api.Features.Exercises.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Exercises
{
    public interface IExerciseRepository
    {
        ExerciseGetDto CreateExercise(ExerciseCreateDto exerciseCreateDto);
        List<ExerciseGetDto> GetAllExercises(int userId);
        ExerciseGetDto EditExercise(int userId, ExerciseEditDto exerciseUpdateDto);
        ExerciseGetDto GetExercise(int exerciseId);
        void DeleteExercise(int exerciseId);
    }

    public class ExerciseRepository : IExerciseRepository
    {
        private readonly DataContext _context;

        public ExerciseRepository(DataContext context)
        {
            _context = context;
        }

        public ExerciseGetDto CreateExercise(ExerciseCreateDto exerciseCreateDto)
        {
            var exercise = new Exercise
            {
                Name = exerciseCreateDto.Name,     
                MuscleGroup = exerciseCreateDto.MuscleGroup,
                UserId = exerciseCreateDto.UserId
            };

            _context.Set<Exercise>().Add(exercise);
            _context.SaveChanges();

            var exerciseGetDto = new ExerciseGetDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup,
                UserId = exercise.Id
            };

            exerciseGetDto.Id = exercise.Id;

            return exerciseGetDto;
        }

        public ExerciseGetDto EditExercise(int userId, ExerciseEditDto exerciseEditDto)
        {
            var exercise = _context.Set<Exercise>().Find(userId);
           
            exercise.Name = exerciseEditDto.Name;
            exercise.MuscleGroup = exerciseEditDto.MuscleGroup;

            _context.SaveChanges();

            var exerciseGetDto = new ExerciseGetDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                MuscleGroup = exercise.MuscleGroup,
                UserId = exercise.Id
            };

            exerciseGetDto.Id = exercise.Id;

            return exerciseGetDto;
        }

        public List<ExerciseGetDto> GetAllExercises(int userId)
        {
            return _context
            .Set<Exercise>()
            .Where(x => x.UserId == userId)
            .Select(x => new ExerciseGetDto
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId
            })
            .ToList();                        
        }

        public ExerciseGetDto GetExercise(int exerciseId)
        {
            return _context
                .Set<Exercise>()
                .Select(x => new ExerciseGetDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    MuscleGroup = x.MuscleGroup,
                    UserId = x.Id
                })

                .FirstOrDefault(x => x.Id == exerciseId);
        }

        public void DeleteExercise(int exerciseId)
        {
            var exercise = _context.Set<Exercise>().Find(exerciseId);
            _context.Set<Exercise>().Remove(exercise);
            _context.SaveChanges();
        }
    }
}
