 using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using System.Linq;

namespace StarterProject.Api.Features.Exercises.Dtos
{
    public class ExerciseCreateDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Name { get; set; }
        public string MuscleGroup { get; set; }
    }

    public class ExerciseCreateDtoValidator : AbstractValidator<ExerciseCreateDto>
    {
        public ExerciseCreateDtoValidator(DataContext dataContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.MuscleGroup)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(x => dataContext.Users.Any(y => y.Id == x))
                .WithMessage("The 'User' does not exist.");

            RuleFor(x => x)
                .Must(dto => !dataContext.Exercises.Any(y => y.Name == dto.Name && y.UserId == dto.UserId))
                .WithMessage(x => $"You already have an exercise of '{x.Name}'")
                .OverridePropertyName(nameof(ExerciseCreateDto.Name));

        }
    }
}