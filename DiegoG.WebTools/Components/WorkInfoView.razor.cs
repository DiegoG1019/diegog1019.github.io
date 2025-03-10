using DiegoG.WebTools.Data;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Globalization;

namespace DiegoG.WebTools.Components;

public partial class WorkInfoView
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; } = default!;

    [Parameter]
    [field: AllowNull]
    public WorkInfoItem Item
    {
        get
        {
            Debug.Assert(field is not null);
            return field;
        }

        set
        {
            ArgumentNullException.ThrowIfNull(value);
            field = value;
        }
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
