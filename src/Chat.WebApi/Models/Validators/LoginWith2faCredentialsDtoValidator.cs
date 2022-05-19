using Chat.Core.Models.Requests;
using FluentValidation;
using System.Linq;

namespace Chat.WebApi.Models.Validators;

public class LoginWith2faCredentialsDtoValidator : AbstractValidator<LoginWith2faCredentialsDto>
{
    public LoginWith2faCredentialsDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();

        RuleFor(x => x.AuthenticatorCode)
            .NotEmpty()
            .Length(6)
            .Custom((value, validation) =>
            {
                if (value.Any(c => !char.IsNumber(c)))
                    validation.AddFailure("AuthenticatiorCode can only contain numbers");
            });
    }
}
