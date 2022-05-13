using static Chat.DI.DI;
using ApplicationViewModel = Chat.ViewModels.Application.ApplicationViewModel;
using SettingsViewModel = Chat.ViewModels.Application.SettingsViewModel;

namespace Chat.ViewModels.Wpf;

public class ViewModelLocator
{
    public static ViewModelLocator Instance { get; private set; } = new();

    public ApplicationViewModel ApplicationViewModel => ViewModelApplication;

    public SettingsViewModel SettingsViewModel => ViewModelSettings;
}
