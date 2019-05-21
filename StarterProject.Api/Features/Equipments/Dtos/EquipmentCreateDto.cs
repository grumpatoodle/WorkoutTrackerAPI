using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using StarterProject.Api.Data.Entites;
using System.Linq;

namespace StarterProject.Api.Features.Equipments.Dtos
{
    public class EquipmentCreateDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Name { get; set; }
    }

    public class EquipmentCreateDtoValidator : AbstractValidator<EquipmentCreateDto>
    {
        public EquipmentCreateDtoValidator(DataContext dataContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(x => dataContext.Users.Any(y => y.Id == x))
                .WithMessage("The 'User' does not exist.");

            RuleFor(x => x)
                .Must(dto => !dataContext.Equipments.Any(y => y.Name == dto.Name && y.UserId == dto.UserId))
                .WithMessage(x => $"You already have the equipment '{x.Name}'")
                .OverridePropertyName(nameof(EquipmentCreateDto.Name));
        }
    }
}
