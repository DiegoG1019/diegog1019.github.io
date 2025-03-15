using DiegoG.WebTools.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace DiegoG.WebTools.Services;

public static class LocalizationHelper
{
    private static CultureInfo? loccult;

    public static CultureInfo LocationCulture
    {
        get => loccult ?? throw new InvalidOperationException("Can't obtain location culture without first fetching it");
        set => loccult = value;
    }

    public static async Task<CultureInfo?> GetLocationRelevantCulture(CachingApiClient<IpInfo> client)
    {
        try
        {
            if (loccult is not null) return LocationCulture;

            var ipInfo = await client.FetchInfo();
            var countryCode = ipInfo?.Country ?? "US";

            return LocationCulture = CultureInfo.GetCultureInfo(countryCode);
        }
        catch (HttpRequestException) { return null; }
    }
}

