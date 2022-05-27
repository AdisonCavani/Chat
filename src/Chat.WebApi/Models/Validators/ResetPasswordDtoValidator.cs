using System;
using Chat.Core.Models.Requests;
using FluentValidation;
using System.Linq;

namespace Chat.WebApi.Models.Validators;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Token)
            .NotEmpty()
            .Custom((context, validation) =>
            {
                if (context.Length != 6)
                    validation.AddFailure("Token", "Token must be in 6 digit format");
            })
            .Custom((context, validation) =>
            {
                if (!context.All(char.IsNumber))
                    validation.AddFailure("Token", "Token can only contain digits");
            });

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            .Custom((value, validation) =>
            {
                if (value.All(char.IsLetterOrDigit))
                    validation.AddFailure("Password must contain at least one non alphanumeric character.");
            });

        // TODO: Add better password validation
    }
}
