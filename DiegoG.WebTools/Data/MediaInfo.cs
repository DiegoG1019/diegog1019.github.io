namespace DiegoG.WebTools.Data;

public enum MediaInfoType
{
    Video,
    Image
}

public sealed class MediaInfo
{
    public Dictionary<string, string> LocalizedTitles { get; init; }
    public string MediaUri { get; init; }
    public MediaInfoType MediaType { get; init; }
}
