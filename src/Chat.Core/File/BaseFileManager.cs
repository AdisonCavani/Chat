using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Chat.Core.CoreDependencyInjection;

namespace Chat.Core;

/// <inheritdoc cref="IFileManager"/>
public class BaseFileManager : IFileManager
{
    /// <inheritdoc/>
    public async Task WriteTextToFileAsync(string text, string path, bool append = false)
    {
        // TODO: Add exception catching

        // Normalize path
        path = NormalizePath(path);

        // Resolve to absolute path
        path = ResolvePath(path);

        // Lock the task
        await AsyncAwaiter.AwaitAsync(nameof(BaseFileManager) + path, async () =>
        {
            // Run the synchronous file access as a new task
            await TaskManager.Run(() =>
            {
                // Write the log message to file
                using (var fs = (TextWriter)new StreamWriter(File.Open(path, append ? FileMode.Append : FileMode.Create)))
                {
                    fs.Write(text);
                }
            });
        });
    }

    /// <inheritdoc/>
    public string NormalizePath(string path)
    {
        // Windows
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return path?.Replace('/', '\\').Trim();
        // Linux/MacOS
        else
            return path?.Replace('\\', '/').Trim();
    }

    /// <inheritdoc/>
    public string ResolvePath(string path) => Path.GetFullPath(path);
}
