using DiegoG.WebTools.DTOs;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Buffers;
using System.Buffers.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace DiegoG.WebTools.Pages;

public partial class QRCodeGeneratorTool
{
    public const string B64ImagePreamble = "data:image/png;base64,";
    private IBrowserFile? File;
    public string ImageData = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==";

    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    private GenerateQRCodeRequest Request { get; } = new();

    private void LoadFile(InputFileChangeEventArgs e)
    {
        Logger.LogInformation("Receiving File upload");
        File = e.File;
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

    public Task HandleRequestSubmission()
        => GenerateNewQR();

    private async Task GenerateNewQR()
    {
        Logger.LogInformation("Generating new QR Code");
        var data = await QRCodeHelpers.GenerateQrCode(
            Request.Content ?? "",
            Request.ECC,
            Request.EciMode,
            Request.PPM,
            Request.ForceUTF8,
            Request.UTF8BOM,
            Request.RequestedVersion,
            File?.OpenReadStream(1024 * 1024 * 100), // 100MB
            Logger
        );

        Logger.LogDebug("Encoding QR Code into Base64 string");
        var rented = ArrayPool<byte>.Shared.Rent(Base64.GetMaxEncodedToUtf8Length(data.Length));
        try
        {
            var b64r = Base64.EncodeToUtf8(data, rented, out _, out int written);
            Debug.Assert(b64r is OperationStatus.Done);
            var output = rented.AsSpan(0, written);

            var str = new string('\0', Encoding.UTF8.GetCharCount(output) + B64ImagePreamble.Length);
            var mutableStr = MemoryMarshal.AsMemory(str.AsMemory()).Span; // We created this bitch we can do whatever we want! ...only while we have it here
            B64ImagePreamble.CopyTo(mutableStr);
            var utf8Success = Encoding.UTF8.TryGetChars(output, mutableStr[B64ImagePreamble.Length..], out _);
            Debug.Assert(utf8Success);
            ImageData = str; // It's no longer ours and is free into the world, oh how quickly they grow
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented);
        }

        Logger.LogInformation("QR Code Succesfully generated");
    }
}
