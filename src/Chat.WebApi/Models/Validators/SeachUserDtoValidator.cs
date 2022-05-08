using Chat.Core.ApiModels.Contacts;
using FluentValidation;

namespace Chat.WebApi.Models.Validators;

public class SearchUsersDtoValidator : AbstractValidator<SearchUsersDto>
{
    public SearchUsersDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.PhoneNumber)
            .NotEmpty();

        // TODO: Fix validation
    }
}
