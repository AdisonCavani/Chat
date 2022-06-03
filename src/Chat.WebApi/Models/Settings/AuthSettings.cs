namespace Chat.WebApi.Models.Settings;

public class AuthSettings
{
    public string Audience { get; set; }

    public int ExpireMinutes { get; set; }

    public string Issuer { get; set; }

    public string SecretKey { get; set; }
}
