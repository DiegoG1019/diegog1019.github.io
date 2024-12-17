using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;

namespace DiegoG.WebTools;

public partial class App
{
    [CascadingParameter]
    public LanguageProvider Language { get; set; }
}
