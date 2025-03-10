using System.Text.Json.Serialization;

namespace DiegoG.WebTools.Data;

public class ExchangeRateInfo
{
    [JsonPropertyName("result")]
    public string Result { get; set; }

    [JsonIgnore]
    public bool IsSuccess => "success".Equals(Result, StringComparison.OrdinalIgnoreCase);

    [JsonPropertyName("conversion_rates")]
    public Dictionary<string, double> ConversionRates { get; set; }
}
