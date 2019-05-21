using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Exercises.Dtos
{
    public class ExerciseEditDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
    }

    public class ExerciseEditDtoValidator : AbstractValidator<ExerciseEditDto>
    {
        public ExerciseEditDtoValidator(DataContext dataContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.MuscleGroup)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(x => dataContext.Users.Any(y => y.Id == x))
                .WithMessage("'User' does not exist.");

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(id => dataContext.Exercises.Any(x => x.Id == id))
                .WithMessage("Exercise does not exist.")
                .DependentRules(() =>
                {
                    RuleFor(x => x)
                        .Must(dto => dataContext.Exercises.Any(exercise =>
                            exercise.Id == dto.Id && exercise.UserId == dto.UserId))
                        .WithMessage("You cannot edit an 'Exercise' you do not own.")
                        .DependentRules(() =>
                        {
                            RuleFor(x => x)
                                .Must(dto => !dataContext.Exercises.Any(y =>
                                    y.Name == dto.Name && y.UserId == dto.UserId && y.Id != dto.Id))
                                .WithMessage(x => $"You already have an exercise of '{x.Name}'")
                                .OverridePropertyName(nameof(ExerciseEditDto.Name));
                        });
                });
        }
    }
}
