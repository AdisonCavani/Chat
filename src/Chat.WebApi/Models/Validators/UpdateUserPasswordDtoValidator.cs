using Chat.Core.ApiModels.UserProfile;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDto>
{
    public UpdateUserPasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.CurrentPassword)
            .NotEqual(x => x.NewPassword);

        // TODO: add zxcvbn-extra-cs
    }
}
