using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;

namespace DiegoG.WebTools.Layout;

public partial class NavMenu
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; }
}
