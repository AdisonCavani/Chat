namespace Chat.Core;

/// <summary>
/// Helper functions for <see cref="IconType"/>
/// </summary>
public static class IconTypeExtensions
{
    /// <summary>
    /// Converts see <see cref="IconType"/> to a FontAwesome string
    /// </summary>
    /// <param name="type">The type to convert</param>
    /// <returns></returns>
    public static string ToFontAwesome(this IconType type)
    {
        switch (type)
        {
            case IconType.File:
                return "\uf15b";

            case IconType.Picture:
                return "\uf03e";

            // If none found, return null
            default:
                return null;
        }
    }
}
