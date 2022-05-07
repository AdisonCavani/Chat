﻿using Chat.Core;

namespace Chat;

/// <summary>
/// The design-time data for a <see cref="MenuItemViewModel"/>
/// </summary>
public class MenuItemDesignModel : MenuItemViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static MenuItemDesignModel Instance => new();

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public MenuItemDesignModel()
    {
        Text = "Hello World";
        Icon = IconType.File;
    }

    #endregion
}
