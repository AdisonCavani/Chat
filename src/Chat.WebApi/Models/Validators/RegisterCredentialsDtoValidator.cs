using Chat.Core.ApiModels.LoginRegister;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class RegisterCredentialsDtoValidator : AbstractValidator<RegisterCredentialsDto>
{
    public RegisterCredentialsDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");

        // TODO: add zxcvbn-extra-cs
    }
}
