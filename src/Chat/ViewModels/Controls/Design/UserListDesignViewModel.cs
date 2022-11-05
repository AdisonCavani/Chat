using Bogus;
using Chat.Services;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.Xaml.Controls;

namespace Chat.ViewModels.Controls.Design;

public class UserListDesignViewModel : UserListViewModel
{
    public UserListDesignViewModel() : base(
            App.Current.Services.GetRequiredService<Frame>(),
            App.Current.Services.GetRequiredService<UserCredentialsManager>())
    {
        var messages = new Faker<ChatMessageItemViewModel>()
                    .RuleFor(v => v.Message, r => r.Lorem.Sentences())
                    .RuleFor(v => v.Send, r => r.Date.Recent())
                    .RuleFor(v => v.SendByMe, r => r.Random.Bool())
                    .RuleFor(v => v.Readed, r => r.Random.Bool((float)0.9));


        var dataSet = new Faker<UserListItemViewModel>()
            .RuleFor(x => x.FirstName, p => p.Name.FirstName())
            .RuleFor(x => x.LastName, p => p.Name.LastName())
            .RuleFor(x => x.Messages, x => new(x.Make(10, () => messages.Generate())));

        Items = new(dataSet.Generate(10));
    }
}