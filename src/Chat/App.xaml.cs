using Chat.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chat;

sealed partial class App : Application
{
    private readonly IHost _host;

    public IServiceProvider Services { get; private set; }

    public new static App Current => (App)Application.Current;

    public App()
    {
        Services = ConfigureServices().BuildServiceProvider();

        var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "log.txt");

        _host = Host.CreateDefaultBuilder()
            .UseSerilog((host, configuration) =>
            {
                configuration
                    .WriteTo.Debug()
                    .WriteTo.File(path, rollingInterval: RollingInterval.Day)
                    .MinimumLevel.Verbose();
            })
            .ConfigureServices(services =>
            {
                services = ConfigureServices();
            })
            .Build();

        InitializeComponent();
        Suspending += OnSuspending;
    }

    private IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });

        // services.AddSingleton<IFilesService, FilesService>();

        return services;
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs e)
    {
        await _host.StartAsync();

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (Window.Current.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new Frame();

            rootFrame.NavigationFailed += OnNavigationFailed;

            if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                //TODO: Load state from previously suspended application
            }

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }

        if (!e.PrelaunchActivated)
        {
            if (rootFrame.Content is null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }

    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    private async void OnSuspending(object sender, SuspendingEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();

        var deferral = e.SuspendingOperation.GetDeferral();
        //TODO: Save application state and stop any background activity
        deferral.Complete();
    }
}
