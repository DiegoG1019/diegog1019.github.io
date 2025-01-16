using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Net.Http.Json;

namespace DiegoG.WebTools.Layout;

public partial class NavMenu
{
    private class AppReference
    {
        public bool Enabled { get; init; }
        public string DisplayName { get; init; }
        public string Uri { get; init; }
        public string Id { get; init; }
        public Dictionary<string, string>? LocalizedNames { get; init; }
        public string? Styles { get; init; }
    }

    private List<AppReference>? Apps;

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
        using var request = new HttpRequestMessage(HttpMethod.Get, "https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/Apps.json");
        request.SetBrowserRequestCache(BrowserRequestCache.NoCache);
        var response = await Http.SendAsync(request);
        if (response.IsSuccessStatusCode)
            Apps = await response.Content.ReadFromJsonAsync<List<AppReference>>();
    }

    private void Language_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
}
