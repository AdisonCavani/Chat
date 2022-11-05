using Chat.Core.Models.Requests;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class PasswordRecoveryTokenDtoValidator : AbstractValidator<PasswordRecoveryTokenDto>
{
    public PasswordRecoveryTokenDtoValidator()
    {
        Include(new PasswordRecoveryDtoValidator());

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
