using DiegoG.WebTools.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace DiegoG.WebTools.Pages;

public partial class Home
{
    public MarkupString? Markup { get; set; }

    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Language.PropertyChanged += Language_PropertyChanged;
    }

    private async void Language_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
        try
        {
            await InvokeAsync(ReloadAboutMe);
        }
        catch(Exception excp)
        {
            await DispatchExceptionAsync(excp);
        }
    }

    private async Task ReloadAboutMe()
    {
        Log.LogInformation(
            "Reloading 'About Me' section with language {langcode}:{lang} @ {uri}",
            Language.CurrentLanguage.LanguageCode,
            Language.CurrentLanguage.LanguageName,
            Language.CurrentLanguage.AboutMeUri
        );

        var resp = await Http.GetAsync(Language.CurrentLanguage.AboutMeUri);
        Markup = (MarkupString)Markdown.ToHtml(await resp.Content.ReadAsStringAsync());
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ReloadAboutMe();
    }
}
