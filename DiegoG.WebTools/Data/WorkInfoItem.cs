using DiegoG.WebTools.Services;

namespace DiegoG.WebTools.Data;

public class WorkInfoItem : ICatalogItem
{
    public string Alt { get; init; }
    public bool Enabled { get; init; }
    public Dictionary<string, string>? LocalizedNames { get; init; }
    public Dictionary<string, string>? LocalizedDescriptions { get; init; }
    public List<MediaInfo>? AttachedMedia { get; init; }
}

public class PortfolioItem : WorkInfoItem
{
    public bool Secondary { get; init; }
    public Dictionary<string, string>? LocalizedTag { get; init; }
}

public class GigQuestion
{
    public Dictionary<string, string>? LocalizedQuestion { get; init; }
    public Dictionary<string, string>? LocalizedExample { get; init; }
}

public class GigInfoItem : WorkInfoItem
{
    public decimal StartingBidUSD { get; init; }
    public decimal EndingBidUSD { get; init; }
    public List<GigQuestion>? Questions { get; init; }
}