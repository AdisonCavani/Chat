using System.ComponentModel;
using Chat.ViewModel.Application;
using static Chat.DI.DI;

namespace Chat.Controls;

/// <summary>
/// Interaction logic for SettingsControl.xaml
/// </summary>
public partial class SettingsControl
{
    public SettingsControl()
    {
        InitializeComponent();

        // Set data context to settings view model

        // If we are in design mode...
        if (DesignerProperties.GetIsInDesignMode(this))
            // Create new instance of settings view model
            DataContext = new SettingsViewModel();
        else
            DataContext = ViewModelSettings;
    }
}
