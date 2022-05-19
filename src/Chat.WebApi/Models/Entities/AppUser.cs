using Microsoft.AspNetCore.Identity;

namespace Chat.WebApi.Models.Entities;

public class AppUser : IdentityUser<int>
{
    [PersonalData]
    public string FirstName { get; init; }

    [PersonalData]
    public string LastName { get; init; }
}
