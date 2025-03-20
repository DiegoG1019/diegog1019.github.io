using System.Text.RegularExpressions;

namespace DiegoG.WebTools;

public static partial class Regexes
{
    [GeneratedRegex(@"\[(?<text>.+)\]\((?<url>.+)\)")]
    private static partial Regex _markupLink();

    public static Regex MarkUpLink { get; } = _markupLink();
}