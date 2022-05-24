using Chat.Core.Models.Requests;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class PasswordRecoveryDtoValidator : AbstractValidator<PasswordRecoveryDto>
{
    public PasswordRecoveryDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}
