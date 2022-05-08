namespace Chat.Animation;

/// <summary>
/// Styles of page animations for appearing/disappearing
/// </summary>
public enum PageAnimation
{
    /// <summary>
    /// No animation takes place
    /// </summary>
    None,

    /// <summary>
    /// The page slides in and fades in from the right
    /// </summary>
    SlideAndFadeInFromRight,

    /// <summary>
    /// The page slides out and fades out to the left
    /// </summary>
    SlideAndFadeOutToLeft,
}
