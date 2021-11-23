using Microsoft.AspNetCore;
using Dna.AspNet;
using Dna;

namespace Chat.Web.Server;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    public static IWebHost BuildWebHost(string[] args)
    {
        return WebHost.CreateDefaultBuilder()
            .UseDnaFramework(construct =>
            {
                // Configure framework - add logging to file
                construct.AddFileLogger();
            })
            .UseStartup<Startup>()
            .Build();
    }
}