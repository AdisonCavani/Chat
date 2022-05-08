﻿namespace Chat.Core.ApiModels.Contacts;

/// <summary>
/// A single result of searching for users with specific details
/// </summary>
public class SearchUsersResultDto
{
    /// <summary>
    /// The users username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// The users first name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// The users last name
    /// </summary>
    public string LastName { get; set; }
}