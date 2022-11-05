using System;
using System.Collections.Generic;

namespace Chat.Core;

public class ErrorResponse
{
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}
