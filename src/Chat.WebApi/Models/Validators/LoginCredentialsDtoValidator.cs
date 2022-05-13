using Chat.Core.ApiModels.LoginRegister;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class LoginCredentialsDtoValidator : AbstractValidator<LoginCredentialsDto>
{
    public LoginCredentialsDtoValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty();

        When(x => x.UsernameOrEmail is not null && x.UsernameOrEmail.Contains('@'), () =>
        {
            RuleFor(x => x.UsernameOrEmail)
                .EmailAddress();
        });

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
