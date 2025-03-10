using Blazored.Modal.Services;
using DiegoG.WebTools.Data;
using DiegoG.WebTools.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DiegoG.WebTools.Pages;

public partial class ContactMePage
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; } = default!;

    private List<MeansOfContact>? MeansOfContact;
    
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Language.PropertyChanged += Language_PropertyChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        MeansOfContact = await catalog.FetchItems();
    }

    private void Language_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
}
