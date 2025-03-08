namespace DiegoG.WebTools.Data;

public sealed class MeansOfContact
{
    public enum DisplayDataType
    {
        None,
        HTML,
        Image
    }

    public bool Enabled { get; init; }
    public string Alt { get; init; }
    public string DisplayName { get; init; }
    public string Uri { get; init; }
    public DisplayDataType DisplayType { get; init; }
    public string? DisplayData { get; init; }
}
