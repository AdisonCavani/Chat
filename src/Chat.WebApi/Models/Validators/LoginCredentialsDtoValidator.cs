using Chat.Core.Models.Requests;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class LoginCredentialsDtoValidator : AbstractValidator<LoginCredentialsDto>
{
    public LoginCredentialsDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}
