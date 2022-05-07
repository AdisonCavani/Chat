﻿namespace Chat.ViewModel.Dialogs.Design;

/// <summary>
/// The design-time data for a <see cref="MessageBoxDialogViewModel"/>
/// </summary>
public class MessageBoxDialogDesignModel : MessageBoxDialogViewModel
{
    #region Singleton

    /// <summary>
    /// A single instance of the design model
    /// </summary>
    public static MessageBoxDialogDesignModel Instance => new();

    #endregion

    #region Constructor

    /// <summary>
    /// Default Constructor
    /// </summary>
    public MessageBoxDialogDesignModel()
    {
        OkText = "OK";
        Message = "Design time messages are fun :)";
    }

    #endregion
}