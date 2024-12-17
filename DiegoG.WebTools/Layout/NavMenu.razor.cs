using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;

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

    private void Language_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
}
