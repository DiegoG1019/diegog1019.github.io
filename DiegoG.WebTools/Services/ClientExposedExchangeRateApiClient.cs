using Blazored.LocalStorage;
using DiegoG.WebTools.Data;
using System.Diagnostics;
using System.Net.Http.Json;

namespace DiegoG.WebTools.Services;

public sealed class ClientExposedExchangeRateApiClient(
    HttpClient Http, 
    ILocalStorageService Storage, 
    ILogger<ClientExposedExchangeRateApiClient> Logger
)
{
    ExchangeRateInfo? cached;
    private readonly record struct ExchangeRateInfoContainer(ExchangeRateInfo Info, DateTimeOffset DateStored);

    public async Task<ExchangeRateInfo?> FetchInfo()
    {
        const string RequestUri = "https://v6.exchangerate-api.com/v6/973b33f470da8e5c1e1fb7a0/latest/USD";
        const string StorageUri = "DiegoG.WebTools.LocalStorage.ExchangeRateInfo";

        if (cached is not null)
        {
            Logger.LogDebug("Fetching cached exchange rate info");
            return cached;
        }

        try
        {
            var container = await Storage.GetItemAsync<ExchangeRateInfoContainer>(StorageUri);
            if (container.Info is not null && container.DateStored < DateTimeOffset.Now + TimeSpan.FromDays(1))
            {
                Logger.LogDebug("Fetching stored exchange rate info");
                return cached = container.Info;
            }
        }
        catch { }

        var info = await Http.GetFromJsonAsync<ExchangeRateInfo>(RequestUri);
        if (info is not null)
            await Storage.SetItemAsync(StorageUri, new ExchangeRateInfoContainer(info, DateTimeOffset.Now));

        Logger.LogDebug("Fetched and refreshed exchange rate info");
        cached = info;

        Debug.Assert(cached is not null);
        return info;
    }
}
