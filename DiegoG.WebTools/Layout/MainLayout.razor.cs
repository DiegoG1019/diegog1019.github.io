using Blazored.Modal;
using Blazored.Modal.Services;
using DiegoG.WebTools.Pages;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace DiegoG.WebTools.Layout;

public partial class MainLayout
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    [CascadingParameter]
    public IModalService Modal { get; set; } = default!;

    private readonly static ModalOptions ModalOptions = new()
    {
        AnimationType = ModalAnimationType.FadeInOut,
        Class = "size-medium contact-me-modal blazored-modal",
        Size = ModalSize.Medium
    };

    private void ShowContactModal()
        => Modal.Show<ContactMePage>(Language.CurrentLanguage.ContactMeans, ModalOptions);

    private string clang;
    public string CurrentLanguage
    {
        get => clang;
        set 
        {
            Language.CurrentLanguage = AvailableLanguages.Languages[clang = value];
            StateHasChanged();
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        clang = Language.CurrentLanguage.LanguageCode;
    }
}
