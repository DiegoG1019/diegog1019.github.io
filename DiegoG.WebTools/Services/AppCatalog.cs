using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace DiegoG.WebTools.Services;

public sealed class AppReference
{
    public bool Enabled { get; init; }
    public string DisplayName { get; init; }
    public string Uri { get; init; }
    public string Id { get; init; }
    public Dictionary<string, string>? LocalizedNames { get; init; }
    public string? Styles { get; init; }
}

public sealed class AppCatalog(HttpClient Http)
{
    private ImmutableArray<AppReference>? Apps;
    private DateTime lastRefreshed;

    public bool TryGetApps([MaybeNullWhen(false)] out ImmutableArray<AppReference> apps)
    {
        if (Apps is not ImmutableArray<AppReference> _apps || _apps.Length is 0)
        {
            apps = default;
            return false;
        }
        else
        {
            apps = _apps;
            return true;
        }
    }

    public async ValueTask<ImmutableArray<AppReference>> FetchApps()
    {
        if (Apps is ImmutableArray<AppReference> arr && lastRefreshed + TimeSpan.FromMinutes(10) >= DateTime.Now)
            return arr;

        using var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/Apps.json");
        request.SetBrowserRequestCache(BrowserRequestCache.NoCache);
        var response = await Http.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var apps = await response.Content.ReadFromJsonAsync<ImmutableArray<AppReference>>();
            Apps = apps;
            lastRefreshed = DateTime.Now;
            return apps;
        }
        return default;
    }
}
