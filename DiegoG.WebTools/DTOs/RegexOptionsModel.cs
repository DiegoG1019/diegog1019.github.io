using Microsoft.AspNetCore.Components;
using System.Text;
using System.Text.RegularExpressions;

namespace DiegoG.WebTools.DTOs;

public record struct MatchedTextRange(Range Range, bool IsHighlighted)
{
    [ThreadStatic]
    private static StringBuilder? Sb;

    public static StringBuilder GetRegexHighlighterBuilder() => (Sb ??= new()).Clear();
}

public sealed class TestRegexMatchModel(RegexOptionsModel options)
{
    public string? Expression { get; set { field = value; Evaluate(); } }
    public string? Target { get; set { field = value; Evaluate(); } }
    public MatchCollection? Matches { get; private set; }
    public string? Error { get; private set; }

    public MarkupString? HighlightedHtml { get; private set; }

    private void Evaluate()
    {
        if (string.IsNullOrWhiteSpace(Expression) || string.IsNullOrWhiteSpace(Target))
        {
            Matches = null;
            Error = null;
            return;
        }

        try
        {
            var reg = new Regex(Expression, options.EvaluationOptions);
            Matches = reg.Matches(Target);
            Error = null!;

            List<MatchedTextRange> highlightRanges = new();

            var sb = MatchedTextRange.GetRegexHighlighterBuilder();
            int? start = null;
            foreach (var current in Matches.Cast<Match>())
            {
                if (start is not int st)
                {
                    int end = 0;
                    foreach (var comparison in Matches.Cast<Match>())
                    {

                        if (comparison.Index >= current.Index && comparison.Index < GetEndIndex(current) && GetEndIndex(comparison) > end) 
                            end = GetEndIndex(comparison);
                    }

                    sb.Append("<span class=\"regex-highlight\">").Append(current.ValueSpan).Append("</span>");
                    start = end;
                }
                else
                {
                    if (current.Index > st)
                    {
                        start = null;
                        sb.Append(Target.AsSpan(st, current.Index - 1));
                    }
                }
            }

            HighlightedHtml = (MarkupString)sb.ToString();
        }
        catch(Exception e)
        {
            Error = e.Message;
        }

        int GetEndIndex(Match match) => match.Index + match.Length;
    }
}

public sealed class TestRegexSplitModel(RegexOptionsModel options)
{
    public string? Expression { get; set { field = value; Evaluate(); } }
    public string? Target { get; set { field = value; Evaluate(); } }
    public string[]? Final { get; private set; }
    public string? Error { get; private set; }

    private void Evaluate()
    {
        if (string.IsNullOrWhiteSpace(Expression) || string.IsNullOrWhiteSpace(Target))
        {
            Final = null;
            Error = null;
            return;
        }

        try
        {
            var reg = new Regex(Expression, options.EvaluationOptions);
            Final = reg.Split(Target);
            Error = null!;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

public sealed class TestRegexReplacementModel(RegexOptionsModel options)
{
    public string? Expression { get; set { field = value; Evaluate(); } }
    public string? Replacement { get; set { field = value; Evaluate(); } }
    public string? Target { get; set { field = value; Evaluate(); } }
    public string? Final { get; private set; }
    public string? Error { get; private set; }

    private void Evaluate()
    {
        if (string.IsNullOrWhiteSpace(Expression) || string.IsNullOrWhiteSpace(Target) || string.IsNullOrWhiteSpace(Replacement))
        {
            Final = null!;
            Error = null;
            return;
        }

        try
        {
            var reg = new Regex(Expression, options.EvaluationOptions);
            Final = reg.Replace(Target, Replacement);
            Error = null!;
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }
}

public sealed class RegexOptionsModel
{
    public RegexOptions Options { get; set; }

    public RegexOptions EvaluationOptions
        => Options & ~(RegexOptions.Compiled);

    public bool None
    {
        get => Options.HasFlag(RegexOptions.None);
        set
        {
            if (value)
                Options |= RegexOptions.None;
            else
                Options &= ~RegexOptions.None;
        }
    }

    public bool IgnoreCase
    {
        get => Options.HasFlag(RegexOptions.IgnoreCase);
        set 
        {
            if (value)
                Options |= RegexOptions.IgnoreCase;
            else
                Options &= ~RegexOptions.IgnoreCase;
        }
    }

    public bool Multiline
    {
        get => Options.HasFlag(RegexOptions.Multiline);
        set 
        {
            if (value)
                Options |= RegexOptions.Multiline;
            else
                Options &= ~RegexOptions.Multiline;
        }
    }

    public bool ExplicitCapture
    {
        get => Options.HasFlag(RegexOptions.ExplicitCapture);
        set 
        {
            if (value)
                Options |= RegexOptions.ExplicitCapture;
            else
                Options &= ~RegexOptions.ExplicitCapture;
        }
    }

    public bool Compiled
    {
        get => Options.HasFlag(RegexOptions.Compiled);
        set 
        {
            if (value)
                Options |= RegexOptions.Compiled;
            else
                Options &= ~RegexOptions.Compiled;
        }
    }

    public bool Singleline
    {
        get => Options.HasFlag(RegexOptions.Singleline);
        set 
        {
            if (value)
                Options |= RegexOptions.Singleline;
            else
                Options &= ~RegexOptions.Singleline;
        }
    }

    public bool IgnorePatternWhitespace
    {
        get => Options.HasFlag(RegexOptions.IgnorePatternWhitespace);
        set 
        {
            if (value)
                Options |= RegexOptions.IgnorePatternWhitespace;
            else
                Options &= ~RegexOptions.IgnorePatternWhitespace;
        }
    }

    public bool RightToLeft
    {
        get => Options.HasFlag(RegexOptions.RightToLeft);
        set 
        {
            if (value)
                Options |= RegexOptions.RightToLeft;
            else
                Options &= ~RegexOptions.RightToLeft;
        }
    }

    public bool ECMAScript
    {
        get => Options.HasFlag(RegexOptions.ECMAScript);
        set 
        {
            if (value)
                Options |= RegexOptions.ECMAScript;
            else
                Options &= ~RegexOptions.ECMAScript;
        }
    }

    public bool CultureInvariant
    {
        get => Options.HasFlag(RegexOptions.CultureInvariant);
        set 
        {
            if (value)
                Options |= RegexOptions.CultureInvariant;
            else
                Options &= ~RegexOptions.CultureInvariant;
        }
    }

    public bool NonBacktracking
    {
        get => Options.HasFlag(RegexOptions.NonBacktracking);
        set 
        {
            if (value)
                Options |= RegexOptions.NonBacktracking;
            else
                Options &= ~RegexOptions.NonBacktracking;
        }
    }
}
