namespace DiegoG.WebTools;

public static class WebToolsExtensions
{
    public static IEnumerable<string> SplitParagraphs(this string str)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(str);
        int s = 0;
        for (int i = 1; i < str.Length; i++)
            if (str[i] == '\n')
            {
                yield return str.Substring(s, (i) - s);
                s = i;
            }
        yield return str.Substring(s, str.Length - s);
    }

    public static string RenderMarkupLinks(this string str)
        => Regexes.MarkUpLink.Replace(str, @"<a href=""$2"" target=""_blank"">$1</a>");
}
