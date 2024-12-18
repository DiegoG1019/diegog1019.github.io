using DiegoG.WebTools.DTOs;
using DiegoG.WebTools.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using QRCoder;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;

namespace DiegoG.WebTools.Pages;

public partial class QRCodeGeneratorTool
{
    private static readonly ImmutableArray<(QRCodeGenerator.ECCLevel Value, string Name)> ECCLevels = [
        (QRCodeGenerator.ECCLevel.Default, "Default"),
        (QRCodeGenerator.ECCLevel.L, "L"),
        (QRCodeGenerator.ECCLevel.M, "M"),
        (QRCodeGenerator.ECCLevel.Q, "Q"),
        (QRCodeGenerator.ECCLevel.H, "H")
    ];

    private static readonly ImmutableArray<(QRCodeGenerator.EciMode Value, string Name)> EciModes = [
        (QRCodeGenerator.EciMode.Default, "Default"),
        (QRCodeGenerator.EciMode.Iso8859_1, "ISO 8859 1"),
        (QRCodeGenerator.EciMode.Iso8859_2, "ISO 8859 2"),
        (QRCodeGenerator.EciMode.Utf8, "UTF-8")
    ];

    public const string B64ImagePreamble = "data:image/png;base64,";
    private IBrowserFile? File;
    public string ImageData = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg==";

    [CascadingParameter]
    public LanguageProvider Language { get; set; }

    private GenerateQRCodeRequest Request { get; } = new();

    private string QRCodeSizeNotice { get; set; } = "N/A";

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

    private void WriteQrCodeB64(byte[] data)
    {
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
    }

    private async Task GenerateNewQR()
    {
        Logger.LogInformation("Generating new QR Code");
        var (data, bounds) = await QRCodeHelpers.GenerateQrCode(
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
        WriteQrCodeB64(data);
        QRCodeSizeNotice = $"{bounds.Width} x {bounds.Height} @ {data.Length / 1024f:0.00} KB";

        Logger.LogInformation("QR Code Succesfully generated");
    }
}
