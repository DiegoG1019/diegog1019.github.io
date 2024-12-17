using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;

namespace DiegoG.WebTools.Layout;

public partial class MainLayout
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; }
}
