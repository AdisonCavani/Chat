﻿namespace Chat.Core;

/// <summary>
/// Helper functions for <see cref="IconType"/>
/// </summary>
public static class IconTypeExtensions
{
    /// <summary>
    /// Converts <see cref="IconType"/> to a FontAwesome string
    /// </summary>
    /// <param name="type">The type to convert</param>
    /// <returns></returns>
    public static string ToFontAwesome(this IconType type)
    {
        // Return a FontAwesome string based on the icon type
        return type switch
        {
            IconType.File => "\uf0f6",
            IconType.Picture => "\uf1c5",
            // If none found, return null
            _ => null,
        };
    }
}
