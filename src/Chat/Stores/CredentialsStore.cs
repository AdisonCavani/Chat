using Chat.Models;
using System;

namespace Chat.Stores;

public class CredentialsStore
{
    public event Action<Credentials> CredentialsUpdated;

    public void UpdateCredentials(Credentials credentials)
    {
        CredentialsUpdated?.Invoke(credentials);
    }

    public void ClearCredentials()
    {
        CredentialsUpdated?.Invoke(new Credentials());
    }
}
