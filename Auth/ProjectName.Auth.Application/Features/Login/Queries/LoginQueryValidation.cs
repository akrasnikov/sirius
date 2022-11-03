using FluentValidation;

namespace ProjectName.Auth.Application.Features.Login.Queries
{
    public class LoginQueryValidation : AbstractValidator<LoginQuery>
    {
        public LoginQueryValidation()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
