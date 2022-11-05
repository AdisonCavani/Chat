using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Chat.WPF;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<MainViewModel>();
        services.AddScoped<MainWindow>();

        var sp = services.BuildServiceProvider();

        sp.GetRequiredService<MainWindow>().Show();
    }
}
