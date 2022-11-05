using Chat.ApiSDK;
using Chat.Db.Models.App;
using Chat.Services;
using Chat.Stores;
using Chat.ViewModels;
using Chat.ViewModels.Controls.Design;
using Chat.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Chat;

sealed partial class App : Application
{
    public IServiceProvider Services { get; }

    public new static App Current => (App)Application.Current;

    public App()
    {
        AddSerilog();
        Services = ConfigureServices().BuildServiceProvider();

        InitializeComponent();
        Suspending += OnSuspending;
    }

    void AddSerilog()
    {
        var path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "log.txt");

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Debug()
            .WriteTo.File(path, rollingInterval: RollingInterval.Day)
            .MinimumLevel.Verbose()
            .CreateLogger();
    }

    IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog();
        });

        services.AddSingleton<Frame>();

        services.AddSingleton<ChatViewModel>();
        services.AddSingleton<LoginViewModel>();
        services.AddSingleton<RegisterViewModel>();
        services.AddSingleton<RecoverPasswordViewModel>();

        services.AddSingleton<InfobarStore>();
        services.AddSingleton<CredentialsStore>();

        services.AddScoped<UserCredentialsManager>();

        // Design view models
        services.AddSingleton<UserListDesignViewModel>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite("Data Source=Chat.db"); // TODO: configure this with IOptions
        });

        // TODO: configure this with IOptions
        services.AddRefitClient<IAccountApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new("https://localhost:5001"));
        services.AddRefitClient<IPasswordApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new("https://localhost:5001"));
        services.AddRefitClient<IProfileApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new("https://localhost:5001"));

        return services;
    }

    async Task ConfigureApplicationAsync()
    {
        var context = Services.GetRequiredService<UserCredentialsManager>();
        var logger = Services.GetRequiredService<ILogger<App>>();

        var createdDb = await context.EnsureCreatedAsync();
        await context.EnsureCreatedAsync();

        if (createdDb)
            logger.LogInformation("Created new Sqlite database");

        // TODO: better method?
        // Initialize singleton with stores
        Services.GetRequiredService<RecoverPasswordViewModel>();
    }

    async Task Navigate()
    {
        var context = Services.GetRequiredService<UserCredentialsManager>();
        var frame = Services.GetRequiredService<Frame>();

        var hasCredentials = await context.GetUserCredentialsAsync();

        frame.Navigate(
            hasCredentials is null ?
            typeof(LoginPage) :
            typeof(ChatPage));
    }

    protected override async void OnLaunched(LaunchActivatedEventArgs e)
    {
        await ConfigureApplicationAsync();

        CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
        ApplicationView.GetForCurrentView().SetPreferredMinSize(new(500, 469));

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (Window.Current.Content is not Frame rootFrame)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = Services.GetRequiredService<Frame>();

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
                await Navigate();
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }

    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();

        //TODO: Save application state and stop any background activity
        deferral.Complete();
    }
}
