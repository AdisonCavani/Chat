using System.Threading.Tasks;
using System.Windows;
using Chat.Controls;
using Chat.Core.DataModels;
using Chat.Core.DI.Interfaces;
using Chat.Core.File;
using Chat.DI.UI;
using Chat.Relational;
using Chat.ViewModels.Application;
using Chat.ViewModels.Wpf;
using Chat.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Chat;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    private readonly IHost _host;
    private readonly ILogger<App> _logger;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .UseSerilog((host, loggerConfiguration) =>
            {
                loggerConfiguration
#if DEBUG
                    .MinimumLevel.Verbose()
                    .WriteTo.Debug()
#endif
                    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<ApplicationViewModel>();
                services.AddSingleton<SettingsViewModel>();
                services.AddSingleton<WindowViewModel>();
                services.AddSingleton<LoginPage>(s => new()
                {
                    DataContext = s.GetRequiredService<LoginViewModel>()
                });

                services.AddSingleton<SettingsControl>(s => new()
                {
                    DataContext = s.GetRequiredService<SettingsViewModel>()
                });

                services.AddSingleton<MainWindow>(s => new()
                {
                    DataContext = s.GetRequiredService<WindowViewModel>()
                });

                services.AddTransient<IFileManager, BaseFileManager>();
                services.AddTransient<IUIManager, UIManager>();

                services.AddDbContext<ClientDataStoreDbContext>(options =>
                {
                    options.UseSqlite("Data Source=Chat.db");
                }, contextLifetime: ServiceLifetime.Transient);

                services.AddTransient<IClientDataStore>(provider =>
                new BaseClientDataStore(provider.GetRequiredService<ClientDataStoreDbContext>()));

            }).Build();

        _logger = _host.Services.GetRequiredService<ILogger<App>>();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await _host.StartAsync();

        await ApplicationSetupAsync();

        _logger.LogDebug("Application starting...");

        _host.Services.GetRequiredService<ApplicationViewModel>().GoToPage(
            await _host.Services.GetRequiredService<IClientDataStore>().HasCredentialsAsync() ?
            ApplicationPage.Chat :
            ApplicationPage.Login);

        Current.MainWindow = _host.Services.GetRequiredService<MainWindow>();
        Current.MainWindow.Show();

        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        base.OnExit(e);
    }

    private async Task ApplicationSetupAsync()
    {
        // Ensure the client data store 
        await _host.Services.GetRequiredService<IClientDataStore>().EnsureDataStoreAsync();

        // Monitor for server connection status
        MonitorServerStatus();

        // Load new settings
        await Task.Run(_host.Services.GetRequiredService<SettingsViewModel>().Load);
    }

    private void MonitorServerStatus()
    {
        // Create a new endpoint watcher
        _ = new Dna.HttpEndpointChecker(
            // Checking fasetto.chat
            // Configuration["FasettoWordServer:HostUrl"],
            "https://google.com",
            // Every 20 seconds
            interval: 20000,
            // Pass in the DI logger
            logger: _logger,
            // On change...
            stateChangedCallback: (result) =>
            {
                // Update the view model property with the new result
                _host.Services.GetRequiredService<ApplicationViewModel>().ServerReachable = result;
            });
    }
}
