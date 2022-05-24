using Chat.Core.Models.Requests;
using FluentValidation;
using System.Linq;

namespace Chat.WebApi.Models.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            .Custom((value, validation) =>
            {
                if (!value.Any(c => !char.IsLetterOrDigit(c)))
                    validation.AddFailure("Password must contain at least one non alphanumeric character.");
            });

        // TODO: Add better password validation
    }
}
