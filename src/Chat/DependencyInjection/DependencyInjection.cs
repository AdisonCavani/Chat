using Chat.Core;
using Dna;

namespace Chat;

/// <summary>
/// A shorthand access class to get DI services with a nice clean short code
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// A shortcut to access the <see cref="IUIManager"/>
    /// </summary>
    public static IUIManager UI => Framework.Service<IUIManager>();

    /// <summary>
    /// A shortcut to access the <see cref="ApplicationViewModel"/>
    /// </summary>
    public static ApplicationViewModel ViewModelApplication => Framework.Service<ApplicationViewModel>();

    /// <summary>
    /// A shortcut to access the <see cref="ProfileViewModel"/>
    /// </summary>
    public static ProfileViewModel ViewModelProfile => Framework.Service<ProfileViewModel>();

    /// <summary>
    /// A shortcut to access the <see cref="IClientDataStore"/>
    /// </summary>
    public static IClientDataStore ViewModelClientDataStore => Framework.Service<IClientDataStore>();
}
