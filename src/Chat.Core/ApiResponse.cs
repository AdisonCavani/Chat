using System;
using System.Collections.Generic;

namespace Chat.Core;

public class ApiResponse
{
    public IEnumerable<string>? Errors { get; set; } = Array.Empty<string>();

    public object? Result { get; set; } = default!;
}

public class ApiResponse<T>
{
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();

    public T? Result { get; set; } = default!;
}
