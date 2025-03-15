using Blazored.LocalStorage;
using Blazored.Modal;
using DiegoG.WebTools.Data;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection.Metadata;

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

        builder.Services.AddCascadingValue(sp => new LanguageProvider(sp.GetRequiredService<ILogger<LanguageProvider>>()));
        builder.Services.AddSingleton(new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddItemCatalog<AppReference>("https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/Apps.json");
        builder.Services.AddItemCatalog<MeansOfContact>("https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/ContactMeans.json");
        builder.Services.AddItemCatalog<WorkInfoItem>("https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/Portfolio.json");
        builder.Services.AddItemCatalog<GigInfoItem>("https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/GigInfo.json");
        builder.Services.AddScoped<CachingApiClient<ExchangeRateInfo>>(
            s => new(
                s.GetRequiredService<HttpClient>(),
                s.GetRequiredService<ILocalStorageService>(),
                s.GetRequiredService<ILogger<CachingApiClient<ExchangeRateInfo>>>(),
                "https://v6.exchangerate-api.com/v6/973b33f470da8e5c1e1fb7a0/latest/USD"
            )
        );
        builder.Services.AddScoped<CachingApiClient<IpInfo>>(
            s => new(
                s.GetRequiredService<HttpClient>(),
                s.GetRequiredService<ILocalStorageService>(),
                s.GetRequiredService<ILogger<CachingApiClient<IpInfo>>>(),
                "https://ipinfo.io/?token=ad57862f90815e"
            )
        );
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddBlazoredModal();

#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
#else
        builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif
        await builder.Build().RunAsync();
    }
}
