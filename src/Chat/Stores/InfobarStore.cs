using Chat.Models;
using System;

namespace Chat.Stores;

public class InfobarStore
{
    public event Action<Infobar> InfobarUpdated;

    public void UpdateInfobar(Infobar infobar)
    {
        InfobarUpdated?.Invoke(infobar);
    }

    public void HideInfobar()
    {
        InfobarUpdated?.Invoke(new Infobar());
    }
}
