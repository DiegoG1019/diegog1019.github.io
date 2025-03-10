using DiegoG.WebTools.Data;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DiegoG.WebTools.Components;

public partial class GigInfoView
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; } = default!;

    public bool TryGetExchangedBids(GigInfoItem item, out string exchangedStartingBid, out string exchangedEndingBid, out string currencyCode)
        => TryGetExchangedBids(item.StartingBidUSD, item.EndingBidUSD, out exchangedStartingBid, out exchangedEndingBid, out currencyCode);

    public bool TryGetExchangedBids(decimal startingBid, decimal endingBid, out string exchangedStartingBid, out string exchangedEndingBid, out string currencyCode)
    {
        var ri = new RegionInfo(CultureInfo.CurrentCulture.LCID);
        logger.LogInformation("Obtained RegionInfo from current culture {cc}: {ri}/{cs}", CultureInfo.CurrentCulture.LCID, ri.Name, ri.ISOCurrencySymbol);

        if (ri.ISOCurrencySymbol == "USD")
            logger.LogInformation("Current locale's currency is USD");
            // Here, it jumps to the end of the method

        else if (ExchangeRateInfo?.ConversionRates.TryGetValue(ri.ISOCurrencySymbol, out var conversion) is not true)
            logger.LogInformation("Could not convert and format currency into {cs}", ri.ISOCurrencySymbol);
            // Here, it jumps to the end of the method
        else
        {
            logger.LogInformation("Converted currency into {cs}", ri.ISOCurrencySymbol);
            exchangedStartingBid = (startingBid * (decimal)conversion).ToString("C", CultureInfo.CurrentCulture);
            exchangedEndingBid = (endingBid * (decimal)conversion).ToString("C", CultureInfo.CurrentCulture);
            currencyCode = ri.ISOCurrencySymbol;
            return true;
        }

        exchangedStartingBid = startingBid.ToString("C", LanguageProvider.DefaultEnglishNumberFormatInfo);
        exchangedEndingBid = endingBid.ToString("C", LanguageProvider.DefaultEnglishNumberFormatInfo);
        currencyCode = "USD";
        return false;
    }

    public bool TryGetExchangedValue(decimal value, out string exchanged)
    {
        var ri = new RegionInfo(CultureInfo.CurrentCulture.LCID);
        
        if (ExchangeRateInfo?.ConversionRates.TryGetValue(ri.ISOCurrencySymbol, out var conversion) is not true)
        {
            exchanged = value.ToString("C", CultureInfo.GetCultureInfo(1033));
            return false;
        }
        else
        {
            exchanged = (value * (decimal)conversion).ToString("C", CultureInfo.GetCultureInfo(ri.ThreeLetterISORegionName));
            return true;
        }
    }

    private ExchangeRateInfo? ExchangeRateInfo;

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
