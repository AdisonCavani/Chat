using Chat.Core.ApiModels.LoginRegister;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class LoginCredentialsDtoValidator : AbstractValidator<LoginCredentialsDto>
{
    public LoginCredentialsDtoValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .EmailAddress()
            .When(p => p.UsernameOrEmail.Contains('@'))
            .NotEmpty()
            .When(p => !p.UsernameOrEmail.Contains('@'));

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
