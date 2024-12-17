using DiegoG.WebTools.Services;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace DiegoG.WebTools.Pages;

public partial class Home
{
    public MarkupString? Markup { get; set; }

    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        var resp = await Http.GetAsync(Language.CurrentLanguage.AboutMeUri);
        Markup = (MarkupString)Markdown.ToHtml(await resp.Content.ReadAsStringAsync());
    }
}
