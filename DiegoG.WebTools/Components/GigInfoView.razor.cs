using DiegoG.WebTools.Data;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime;

namespace DiegoG.WebTools.Components;

public partial class GigInfoView
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; } = default!;

    public bool TryGetExchangedBids(GigInfoItem item, out string exchangedStartingBid, out string exchangedEndingBid, out string currencyCode)
        => TryGetExchangedBids(item.StartingBidUSD, item.EndingBidUSD, out exchangedStartingBid, out exchangedEndingBid, out currencyCode);

    public bool TryGetExchangedBids(decimal startingBid, decimal endingBid, out string exchangedStartingBid, out string exchangedEndingBid, out string currencyCode)
    {
        CultureInfo cu;
        RegionInfo ri;
        if (LocalIpInfo is null)
        {
            cu = CultureInfo.CurrentCulture;
            ri = new RegionInfo(cu.LCID);
            logger.LogInformation("Obtained RegionInfo from current culture {cn}|{cc}: {ri}/{ics}/{cs}", cu.Name, cu.LCID, ri.Name, ri.ISOCurrencySymbol, ri.CurrencySymbol);
        }
        else
        {
            cu = CultureInfo.GetCultureInfo(LocalIpInfo!.Country);
            ri = new RegionInfo(cu.Name);
            logger.LogInformation("Obtained RegionInfo from location culture {cn}|{cc}: {ri}/{ics}/{cs}", cu.Name, cu.LCID, ri.Name, ri.ISOCurrencySymbol, ri.CurrencySymbol);
        }

        if (ri.ISOCurrencySymbol == "USD")
            logger.LogInformation("Current locale's currency is USD");
            // Here, it jumps to the end of the method

        else if (ExchangeRateInfo?.ConversionRates.TryGetValue(ri.ISOCurrencySymbol, out var conversion) is not true)
            logger.LogInformation("Could not convert and format currency into {cs}", ri.ISOCurrencySymbol);
            // Here, it jumps to the end of the method
        else
        {
            logger.LogInformation("Converted currency into {cs}", ri.ISOCurrencySymbol);
            exchangedStartingBid = (startingBid * (decimal)conversion).ToString("n", cu);
            exchangedEndingBid = (endingBid * (decimal)conversion).ToString("n", cu);
            currencyCode = ri.ISOCurrencySymbol;
            return true;
        }

        exchangedStartingBid = startingBid.ToString("C", LanguageProvider.DefaultEnglishNumberFormatInfo);
        exchangedEndingBid = endingBid.ToString("C", LanguageProvider.DefaultEnglishNumberFormatInfo);
        currencyCode = "USD";
        return false;
    }

    private ExchangeRateInfo? ExchangeRateInfo;
    private IpInfo? LocalIpInfo;

    [Parameter]
    [field: AllowNull]
    public GigInfoItem Item
    {
        get
        {
            Debug.Assert(field is not null);
            return field;
        }

        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        ExchangeRateInfo = await ExchangeRates.FetchInfo();
        try
        {
            LocalIpInfo = await IpInfo.FetchInfo();
        }
        catch (HttpRequestException)
        {
            logger.LogDebug("Could not obtain location info -- Maybe it was blocked?");
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Language.PropertyChanged += Language_PropertyChanged;
    }

    private void Language_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
}
