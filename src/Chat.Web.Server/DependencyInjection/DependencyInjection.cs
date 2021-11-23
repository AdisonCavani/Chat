using Dna;

namespace Chat.Web.Server;

/// <summary>
/// A shorthand access class to get DI services with nice clean short code
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// The scoped instance of the <see cref="ApplicationDbContext"/>
    /// </summary>
    public static ApplicationDbContext ApplicationDbContext => Framework.Provider.GetService<ApplicationDbContext>();
}
