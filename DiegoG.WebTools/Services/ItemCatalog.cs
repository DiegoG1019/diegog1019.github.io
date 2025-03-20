using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace DiegoG.WebTools.Services;

public interface ICatalogItem
{
    public bool Enabled { get; }
    public string Alt { get; }
}

public class ItemCatalog<T>(HttpClient Http, ILogger<ItemCatalog<T>> logger, string fetchUri) where T : ICatalogItem
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

    public IEnumerable<ArraySegment<T>> GroupAndEnumerateItems(int grouping = 2, bool fillInRow = false, Func<T, bool>? predicate = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(grouping, 1);

        if (Items is not List<T> items)
            yield break;

        logger.LogInformation("Displaying {count} items", items.Count);

        int idx = 0;
        T[] buffer = new T[grouping];
        foreach (var item in (predicate is not null ? items.Where(predicate) : items))
        {
            if (item.Enabled)
            {
                logger.LogDebug("Buffering items: {name}", item.Alt);
                buffer[idx++] = item;
            }
            else
            {
                logger.LogDebug("Skipping disabled items: {name}", item.Alt);
                continue;
            }

            Debug.Assert(idx <= grouping);
            if (idx == grouping)
            {
                idx = 0;
                logger.LogDebug("Flushing {grouping} items", grouping);
                yield return new ArraySegment<T>(buffer, 0, grouping);
            }
        }

        Debug.Assert(idx <= grouping);
        if (idx > 0)
        {
            if (fillInRow)
            {
                for (int i = grouping - 1; i >= idx; i--)
                    buffer[i] = default!;
                idx = grouping;
            }

            logger.LogDebug("Flushing {amount} items", idx);
            yield return new ArraySegment<T>(buffer, 0, idx);
        }
    }
}
