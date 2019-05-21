using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Routines.Dtos
{
    public class RoutineCreateDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    public class RoutineCreateDtoValidator : AbstractValidator<RoutineCreateDto>
    {
        public RoutineCreateDtoValidator(DataContext dataContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(x => dataContext.Users.Any(y => y.Id == x))
                .WithMessage("The 'User' does not exist.");

            RuleFor(x => x)
                .Must(dto => !dataContext.Routines.Any(y => y.Name == dto.Name && y.UserId == dto.UserId))
                .WithMessage(x => $"You already have a routine of '{x.Name}'")
                .OverridePropertyName(nameof(RoutineCreateDto.Name));
        }
    }
}
