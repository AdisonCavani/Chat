﻿using Chat.Core.DI.Interfaces;
using Dna;

namespace Chat.Core.DI;

/// <summary>
/// The IoC container for our application
/// </summary>
public static class CoreDI
{
    /// <summary>
    /// A shortcut to access the <see cref="IFileManager"/>
    /// </summary>
    public static IFileManager FileManager => Framework.Service<IFileManager>();
}
