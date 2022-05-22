using Chat.Core.Models.Requests;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class ResendVerificationEmailDtoValidator : AbstractValidator<ResendVerificationEmailDto>
{
    public ResendVerificationEmailDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}
