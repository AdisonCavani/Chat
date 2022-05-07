﻿using System.Security;
using Chat.ViewModel.Application;
using Chat.ViewModel.Base;

namespace Chat.Pages;

/// <summary>
/// Interaction logic for LoginPage.xaml
/// </summary>
public partial class LoginPage : IHavePassword
{
    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public LoginPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Constructor with specific view model
    /// </summary>
    public LoginPage(LoginViewModel specificViewModel) : base(specificViewModel)
    {
        InitializeComponent();
    }

    #endregion

    /// <summary>
    /// The secure password for this login page
    /// </summary>
    public SecureString SecurePassword => PasswordText.SecurePassword;
}
