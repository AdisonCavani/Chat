using Bogus;

namespace Chat.ViewModels.Controls.Design;

public class UserListItemDesignViewModel : UserListItemViewModel
{
    private readonly Faker _faker = new();

    public UserListItemDesignViewModel()
    {
        FirstName = _faker.Name.FirstName();
        LastName = _faker.Name.LastName();
    }
}
