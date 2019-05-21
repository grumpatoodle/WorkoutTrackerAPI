using FluentValidation;
using Newtonsoft.Json;
using StarterProject.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterProject.Api.Features.Equipments.Dtos
{
    public class EquipmentGetDto
    {
        [JsonIgnore]
        public int UserId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class EquipmentGetDtoValidator : AbstractValidator<EquipmentGetDto>
    {
        public EquipmentGetDtoValidator(DataContext dataContext)
        {
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
                         .WithMessage("Unable to get an 'Equipment' that you do not own.");
                 });
        }
    }
}
