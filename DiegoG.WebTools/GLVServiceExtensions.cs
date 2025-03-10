using DiegoG.WebTools.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiegoG.WebTools;

public static class GLVServiceExtensions
{
    public static IServiceCollection AddItemCatalog<TItem>(this IServiceCollection services, string getRequestUri)
        where TItem : ICatalogItem
    {
        services.AddSingleton<ItemCatalog<TItem>>(
            s => new(
                s.GetRequiredService<HttpClient>(), 
                s.GetRequiredService<ILogger<ItemCatalog<TItem>>>(), 
                getRequestUri
            )
        );

        return services;
    }
}
