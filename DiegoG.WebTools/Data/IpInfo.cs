using System.Text.Json.Serialization;

namespace DiegoG.WebTools.Data;

public record IpInfo(
    [property: JsonPropertyName("ip")] string Ip, 
    [property: JsonPropertyName("hostname")] string Hostname, 
    [property: JsonPropertyName("city")] string City, 
    [property: JsonPropertyName("region")] string Region, 
    [property: JsonPropertyName("country")] string Country, 
    [property: JsonPropertyName("loc")] string Location, 
    [property: JsonPropertyName("org")] string Organization, 
    [property: JsonPropertyName("postal")] string PostalCode, 
    [property: JsonPropertyName("timezone")] string TimeZone
);
