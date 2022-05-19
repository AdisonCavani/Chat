using Chat.Core.Models.Requests;
using Chat.WebApi.Models.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Chat.WebApi.Models.Validators;

public class RegisterCredentialsDtoValidator : AbstractValidator<RegisterCredentialsDto>
{
    public RegisterCredentialsDtoValidator(UserManager<AppUser> userManager)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .Custom(async (value, validation) =>
            {
                var exist = await userManager.FindByEmailAsync(value);

                if (exist is not null)
                    validation.AddFailure("Email", "Email is taken");
            });

        RuleFor(x => x.Password)
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
