using FluentValidation;
using StarterProject.Api.Data;

namespace StarterProject.Api.Dtos.Users
{
    public class UserAuthenticationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserAuthenticationDtoValidator : AbstractValidator<UserAuthenticationDto>
    {
        public UserAuthenticationDtoValidator(DataContext context)
        {
            RuleFor(x => x.Username)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
