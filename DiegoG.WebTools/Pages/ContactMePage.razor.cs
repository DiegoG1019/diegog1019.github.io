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

    private IEnumerable<ArraySegment<MeansOfContact>> EnumerateContactMeans()
    {
        if (MeansOfContact is not List<MeansOfContact> means)
            yield break;

        logger.LogInformation("Displaying {count} contact means", means.Count);

        int idx = 0;
        MeansOfContact[] buffer = new MeansOfContact[2];
        foreach (var method in means)
        {
            if (method.Enabled)
            {
                logger.LogDebug("Buffering contact means: {name}({alt})", method.DisplayName, method.Alt);
                buffer[idx++] = method;
            }
            else
            {
                logger.LogDebug("Skipping disabled contact means: {name}({alt})", method.DisplayName, method.Alt);
                continue;
            }

            Debug.Assert(idx <= 2);
            if (idx == 2) 
            {
                idx = 0;
                logger.LogDebug("Flushing 2 contact means");
                yield return new ArraySegment<MeansOfContact>(buffer, 0, 2);
            }
        }

        Debug.Assert(idx <= 2);
        if (idx > 0)
        {
            logger.LogDebug("Flushing {amount} contact means", idx);
            yield return new ArraySegment<MeansOfContact>(buffer, 0, idx);
        }
    }
}
