using DiegoG.WebTools.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DiegoG.WebTools;

public static class GLVServiceExtensions
{
    public static IServiceCollection AddItemCatalog<TItem>(this IServiceCollection services, string getRequestUri)
    {
        services.AddSingleton<ItemCatalog<TItem>>(s => new(s.GetRequiredService<HttpClient>(), getRequestUri));
        return services;
    }
}
