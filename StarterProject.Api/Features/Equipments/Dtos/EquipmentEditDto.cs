using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Equipments.Dtos
{
    public class EquipmentEditDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class EquipmentEditDtoValidator : AbstractValidator<EquipmentEditDto>
    {
        public EquipmentEditDtoValidator(DataContext dataContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.UserId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(x => dataContext.Users.Any(y => y.Id == x))
                .WithMessage("The 'User' does not exist.");

            RuleFor(x => x.Id)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .Must(id => dataContext.Equipments.Any(x => x.Id == id))
                .WithMessage("This 'Equipment' does not exist.")
                .DependentRules(() =>
                {
                    RuleFor(x => x)
                        .Must(dto => dataContext.Equipments.Any(equipment =>
                            equipment.Id == dto.Id && equipment.UserId == dto.UserId))
                        .WithMessage("You cannot edit an equipment that you do not own.")
                        .DependentRules(() =>
                        {
                            RuleFor(x => x)
                                .Must(dto => !dataContext.Equipments.Any(y =>
                                    y.Name == dto.Name && y.UserId == dto.UserId && y.Id != dto.Id))
                                .WithMessage(x => $"You already have the equipment '{x.Name}")
                                .OverridePropertyName(nameof(EquipmentEditDto.Name));
                        });
                });
        }
    }
}
