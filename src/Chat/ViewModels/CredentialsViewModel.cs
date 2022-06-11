using Chat.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Chat.ViewModels;

public class CredentialsViewModel : ObservableObject
{
    private readonly Credentials _credentials;

    public string Email => _credentials.Email;

    public CredentialsViewModel(Credentials credentials)
    {
        _credentials = credentials;
    }
}
