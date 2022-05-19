using Microsoft.Extensions.Hosting;
using Serilog;

namespace Chat.WebApi.Extensions;

public static class Serilog
{
    public static IHostBuilder ConfigureSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((host, configuration) =>
        {
            configuration // TODO: Configure serilog
#if DEBUG
                .MinimumLevel.Verbose()
                .WriteTo.Console();
#else
                .MinimumLevel.Information()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day);
#endif
        });

        return builder;
    }
}
