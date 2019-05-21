using FluentValidation;
using StarterProject.Api.Common;

namespace StarterProject.Api.Features.Users.Dtos
{
    public class UserRoleEditDto
    {
        public string Role { get; set; }
    }

    public class UserRoleEditDtoValidator : AbstractValidator<UserRoleEditDto>
    {
        public UserRoleEditDtoValidator()
        {
            RuleFor(x => x.Role)
                .Must(BeAKnownRole)
                .WithMessage(ErrorMessages.User.RoleIsNotRecognized);
        }

        private bool BeAKnownRole(string role)
        {
            return role == Constants.Users.Roles.Admin
                   || role == Constants.Users.Roles.User;
        }
    }
}
