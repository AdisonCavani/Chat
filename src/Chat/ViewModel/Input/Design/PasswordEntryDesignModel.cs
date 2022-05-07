﻿namespace Chat.ViewModel.Input.Design;

/// <summary>
/// The design-time data for a <see cref="PasswordEntryViewModel"/>
/// </summary>
public class PasswordEntryDesignModel : PasswordEntryViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static PasswordEntryDesignModel Instance => new();

    #endregion

    #region Constructor

    /// <summary>
    /// Default Constructor
    /// </summary>
    public PasswordEntryDesignModel()
    {
        Label = "Name";
        FakePassword = "********";
    }

    #endregion
}
