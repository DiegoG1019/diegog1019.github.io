using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using System.Text;
using System.Text.RegularExpressions;

namespace DiegoG.WebTools.DTOs;

public record struct MatchedTextRange(Range Range, bool IsHighlighted);

public sealed class TestRegexMatchModel
{
    private readonly StringBuilder sb = new();
    private readonly RegexOptionsModel options;

    public TestRegexMatchModel(RegexOptionsModel options)
    {
        this.options = options;
        options.PropertyChanged += (sender, args) => Evaluate();
    }
    
    public string? Expression { get; set { field = value; Evaluate(); } }
    public string? Target { get; set { field = value; Evaluate(); } }
    public MatchCollection? Matches { get; private set; }
    public string? Error { get; private set; }

    public MarkupString? HighlightStylesHtml { get; private set; }

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

            sb.Clear();
            sb.Append("<style>");

            foreach (var match in Matches.SelectMany(x => x.Groups.Values))
            {
                sb.Append("\n#regex-match-capture-")
                    .Append(match.Index)
                    .Append('-')
                    .Append(match.Length)
                    .Append(":hover {\n\tbackground-color: #1d2633; transform: scale(1.1, 1.1)\n}\n\n");

                for (int i = 0; i < match.Length; i++)
                    sb.Append("\nbody:has(#regex-match-capture-")
                        .Append(match.Index)
                        .Append('-')
                        .Append(match.Length)
                        .Append(":hover)")
                        .Append(" #regex-match-char-").Append(match.Index + i).Append(" , ");
                
                sb.Remove(sb.Length - 3, 3);
                    
                sb.Append(" {\n\tbackground-color: #1d2633; border-bottom: 2px solid #273345 \n}\n\n");
            }
            
            sb.Append("</style>");
            
            HighlightStylesHtml = (MarkupString)sb.ToString();

            /*
             <style>
                   #regex-match-capture-@capture.Index-@capture.Length:hover body:has(#regex-match-char-@((i + capture.Length).ToString())) {
                       background-color: red;
                   }

                   #regex-match-capture-@capture.Index-@capture.Length:hover {
                        background-color: red;
                    }
               </style>
             */
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

public sealed class RegexOptionsModel : INotifyPropertyChanged
{
    public RegexOptions Options
    {
        get;
        set
        {
            SetField(ref field, value);
        }
    }

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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
