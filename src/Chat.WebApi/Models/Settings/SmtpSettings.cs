namespace Chat.WebApi.Models.Settings;

public class SmtpSettings
{
    public int Port { get; set; }

    public string Host { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Name { get; set; }
}
