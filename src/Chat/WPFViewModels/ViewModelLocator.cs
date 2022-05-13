using Chat.ViewModel.Application;
using static Chat.DI.DI;

namespace Chat.WPFViewModels;

public class ViewModelLocator
{
    public static ViewModelLocator Instance { get; private set; } = new();

    public ApplicationViewModel ApplicationViewModel => ViewModelApplication;

    public SettingsViewModel SettingsViewModel => ViewModelSettings;
}
