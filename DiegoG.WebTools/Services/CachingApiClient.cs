using Blazored.LocalStorage;
using DiegoG.WebTools.Data;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;

namespace DiegoG.WebTools.Services;

public class CachingApiClient<T>(
    HttpClient Http,
    ILocalStorageService Storage,
    ILogger<CachingApiClient<T>> Logger,
    string RequestUri
)
{
    T? cached;
    public string StorageUri { get; } = $"DiegoG.WebTools.LocalStorage.CachingApiClient<{typeof(T).AssemblyQualifiedName}>";
    private readonly record struct InfoContainer(T Info, DateTimeOffset DateStored);

    [ThreadStatic]
    private static StringBuilder? UriBuilder;

    public async Task<T?> FetchInfo()
    {
        if (cached is not null)
        {
            Logger.LogDebug("Fetching cached info");
            return cached;
        }

        try
        {
            var container = await Storage.GetItemAsync<InfoContainer>(StorageUri);
            if (container.Info is not null && container.DateStored < DateTimeOffset.Now + TimeSpan.FromDays(1))
            {
                Logger.LogDebug("Fetching stored info");
                return cached = container.Info;
            }
        }
        catch { }

        var info = await Http.GetFromJsonAsync<T>(RequestUri);
        if (info is not null)
            await Storage.SetItemAsync(StorageUri, new InfoContainer(info, DateTimeOffset.Now));

        Logger.LogDebug("Fetched and refreshed info");
        cached = info;

        Debug.Assert(cached is not null);
        return info;
    }
}

