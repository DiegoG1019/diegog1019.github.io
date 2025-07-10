using DiegoG.WebTools.DTOs;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;

namespace DiegoG.WebTools.Pages;

public partial class RegexTester
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    private bool? ActiveTabSet { get; set; }
    private bool ActiveEngineSet { get; set; }

    public string MatchTabClass => TabButtonClass(IsMatch);
    public string ReplaceTabClass => TabButtonClass(IsReplace);
    public string SplitTabClass => TabButtonClass(IsSplit);

    public bool IsMatch => ActiveTabSet == null;
    public bool IsReplace => ActiveTabSet == true;
    public bool IsSplit => ActiveTabSet == false;

    public bool IsDotNet => ActiveEngineSet == false;
    public string DotNetEngineClass => TabButtonClass(IsDotNet);

    private static string TabButtonClass(bool expression) => expression ? "btn-primary tab-selected" : "btn-secondary";

    private void SetToDotNetRegex()
    {
    }

    private void SetToMatch()
    {
        ActiveTabSet = null;
        StateHasChanged();
    }

    private void SetToReplace()
    {
        ActiveTabSet = true;
        StateHasChanged();
    }

    private void SetToSplit()
    {
        ActiveTabSet = false;
        StateHasChanged();
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
