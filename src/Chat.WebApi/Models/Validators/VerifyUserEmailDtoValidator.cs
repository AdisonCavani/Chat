using Chat.Core.ApiModels.UserProfile;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class VerifyUserEmailDtoValidator : AbstractValidator<VerifyUserEmailDto>
{
    public VerifyUserEmailDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.EmailToken)
            .NotEmpty();
    }
}
