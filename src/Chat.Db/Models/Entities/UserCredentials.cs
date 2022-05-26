﻿namespace Chat.Db.Models.Entities;

public class UserCredentials
{
    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Token { get; set; }

    public string RefreshToken { get; set; }
}
