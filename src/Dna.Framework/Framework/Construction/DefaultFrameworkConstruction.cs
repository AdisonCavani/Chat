namespace Dna;

/// <summary>
/// Creates a default framework construction containing all
/// the default configuration and services
/// </summary>
public class DefaultFrameworkConstruction : FrameworkConstruction
{
    #region Constructor

    /// <summary>
    /// Default construction
    /// </summary>
    public DefaultFrameworkConstruction()
    {
        // Configure
        this.Configure()
            // Add default services
            .UseDefaultServices();
    }

    #endregion
}
