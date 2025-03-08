using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace DiegoG.WebTools.Services;

public class ItemCatalog<T>(HttpClient Http, string fetchUri)
{
    private List<T>? Items;
    private DateTime lastRefreshed;

    private string FetchUri
    {
        get => field;
        set
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            field = value;
        }
    } = fetchUri;

    public bool TryGetItems([MaybeNullWhen(false)] out List<T> items)
    {
        if (Items is not List<T> _apps || _apps.Count is 0)
        {
            items = default;
            return false;
        }
        else
        {
            items = _apps;
            return true;
        }
    }

    public async ValueTask<List<T>?> FetchItems()
    {
        if (Items is List<T> arr && lastRefreshed + TimeSpan.FromMinutes(10) >= DateTime.Now)
            return arr;

        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            FetchUri
        );

        request.SetBrowserRequestCache(
            lastRefreshed + TimeSpan.FromMinutes(20) >= DateTime.Now
            ? BrowserRequestCache.Reload
            : BrowserRequestCache.NoCache
        );

        var response = await Http.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var apps = await response.Content.ReadFromJsonAsync<List<T>>();
            Items = apps;
            lastRefreshed = DateTime.Now;
            return apps;
        }
        return default;
    }
}
