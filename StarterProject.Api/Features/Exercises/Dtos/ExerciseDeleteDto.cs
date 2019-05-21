using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Exercises.Dtos
{
    public class ExerciseDeleteDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public int Id { get; set; }
    }

    public class ExerciseDeleteDtoValidator : AbstractValidator<ExerciseDeleteDto>
    {
        public ExerciseDeleteDtoValidator(DataContext dataContext)
        {
            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(x => dataContext.Users.Any(y => y.Id == x))
                .WithMessage("The 'User' does not exist.");

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(id => dataContext.Exercises.Any(x => x.Id == id))
                .WithMessage("'Exercise' does not exist.")
                .DependentRules(() =>
                {
                    RuleFor(x => x)
                        .Must(dto => dataContext.Exercises.Any(exercise => exercise.Id == dto.Id && exercise.UserId == dto.UserId))
                        .WithMessage("You cannot delete an 'Exercise' you do not own.");
                });
        }
    }
}
