using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Chat.Extensions;

public static class SecureStringExtensions
{
    public static string Unsecure(this SecureString secureString)
    {
        // Make sure we have a secure string
        if (secureString == null)
            return string.Empty;

        // Get a pointer for an unsecure string in memory
        var unmanagedString = IntPtr.Zero;

        try
        {
            // Unsecures the password
            unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
            return Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
            // Clean up any memory allocation
            Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
    }
}
