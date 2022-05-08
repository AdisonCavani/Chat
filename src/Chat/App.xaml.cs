using System.Threading.Tasks;
using System.Windows;
using Chat.Core.DataModels;
using Chat.DI;
using Chat.Relational;
using Dna;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using static Chat.Core.DI.CoreDI;
using static Chat.DI.DI;
using static Dna.FrameworkDI;

namespace Chat;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    /// <summary>
    /// Custom startup so we load our IoC immediately before anything else
    /// </summary>
    /// <param name="e"></param>
    protected override async void OnStartup(StartupEventArgs e)
    {
        // Let the base application do what it needs
        base.OnStartup(e);

        // Setup the main application 
        await ApplicationSetupAsync();

        // Log it
        Logger.LogDebugSource("Application starting...");

        // Setup the application view model based on if we are logged in
        ViewModelApplication.GoToPage(
            // If we are logged in...
            await ClientDataStore.HasCredentialsAsync() ?
            // Go to chat page
            ApplicationPage.Chat :
            // Otherwise, go to login page
            ApplicationPage.Login);

        // Show the main window
        Current.MainWindow = new MainWindow();
        Current.MainWindow.Show();
    }

    /// <summary>
    /// Configures our application ready for use
    /// </summary>
    private async Task ApplicationSetupAsync()
    {
        // Setup the Dna Framework
        Framework.Construct<DefaultFrameworkConstruction>()
            .AddFileLogger()
            .AddClientDataStore()
            .AddFasettoWordViewModels()
            .AddFasettoWordClientServices()
            .Build();

        // Ensure the client data store 
        await ClientDataStore.EnsureDataStoreAsync();

        // Monitor for server connection status
        MonitorServerStatus();

        // Load new settings
        await Task.Run(ViewModelSettings.LoadAsync);
    }

    /// <summary>
    /// Monitors if the website is up, running and reachable
    /// by periodically hitting it up
    /// </summary>
    private void MonitorServerStatus()
    {
        // Create a new endpoint watcher
        new HttpEndpointChecker(
            // Checking fasetto.chat
            // Configuration["FasettoWordServer:HostUrl"],
            "https://google.com",
            // Every 20 seconds
            interval: 20000,
            // Pass in the DI logger
            logger: Framework.Provider.GetService<ILogger>(),
            // On change...
            stateChangedCallback: (result) =>
            {
                // Update the view model property with the new result
                ViewModelApplication.ServerReachable = result;
            });
    }
}
