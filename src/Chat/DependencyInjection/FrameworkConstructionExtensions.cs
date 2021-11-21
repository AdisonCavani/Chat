using Chat.Core;
using Dna;
using Microsoft.Extensions.DependencyInjection;

namespace Chat;

/// <summary>
/// Extension methods for the <see cref="FrameworkConstruction"/>
/// </summary>
public static class FrameworkConstructionExtensions
{
    /// <summary>
    /// Injects the view models needed for Chat application
    /// </summary>
    /// <param name="construction"></param>
    /// <returns></returns>
    public static FrameworkConstruction AddChatViewModels(this FrameworkConstruction construction)
    {
        // Bind to a single instance of Application view model
        construction.Services.AddSingleton<ApplicationViewModel>();

        // Bind to a single instance of Settings view model
        construction.Services.AddSingleton<ProfileViewModel>();

        // Return the construction for chaining
        return construction;
    }

    /// <summary>
    /// Injects the Chat client application services needed
    /// </summary>
    /// <param name="construction"></param>
    /// <returns></returns>
    public static FrameworkConstruction AddChatClientServices(this FrameworkConstruction construction)
    {
        // Bind a task manager
        construction.Services.AddTransient<ITaskManager, BaseTaskManager>();

        // Bind a file manager
        construction.Services.AddTransient<IFileManager, BaseFileManager>();

        // Bind a UI Manager
        construction.Services.AddTransient<IUIManager, UIManager>();

        // Return the construction for chaining
        return construction;
    }
}
