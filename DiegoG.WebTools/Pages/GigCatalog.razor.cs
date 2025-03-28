﻿using DiegoG.WebTools.Data;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Runtime;

namespace DiegoG.WebTools.Pages;

public partial class GigCatalog
{

    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    public List<GigInfoItem>? InfoItems;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Language.PropertyChanged += Language_PropertyChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        InfoItems = await Catalog.FetchItems();
    }

    private void Language_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
}
