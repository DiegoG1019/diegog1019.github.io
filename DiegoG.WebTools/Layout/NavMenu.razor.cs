using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;

namespace DiegoG.WebTools.Layout;

public partial class NavMenu
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Language.PropertyChanged += Language_PropertyChanged;
    }

    private string GetAppDisplayName(AppReference app)
        => app.LocalizedNames?.TryGetValue(Language.CurrentLanguage.LanguageCode, out var name) is true ? name : app.DisplayName;

    protected override async Task OnInitializedAsync()
    {
        await Apps.FetchApps();
    }

    private void Language_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
}
