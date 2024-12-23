using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace DiegoG.WebTools;

/*
 * When building, remember:
 * * Either figure out how to disable base url setting by the deployer, or
 * * Add a manual edit after deployment to include the SiteBackdrop and remove the base url setting
 */

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddCascadingValue(sp => new LanguageProvider());
#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
#else
        builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif
        await builder.Build().RunAsync();
    }
}
