using System.Collections.Generic;

namespace Chat.WebApi.Models.Internal;

public class TempAuthenticationResult
{
    public string Token { get; set; }

    public bool Success { get; set; }

    public IEnumerable<string>? Errors { get; set; }
}
