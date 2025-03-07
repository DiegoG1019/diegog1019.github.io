using Blazored.Modal.Services;
using DiegoG.WebTools.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace DiegoG.WebTools.Pages;

public partial class ContactMePage
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; } = default!;
    
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
