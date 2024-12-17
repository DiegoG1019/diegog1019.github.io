using DiegoG.WebTools.Services;
using QRCoder;

namespace DiegoG.WebTools.DTOs;

public sealed class GenerateQRCodeRequest
{
    public string? Content { get; set; }
    public int PPM { get; set; } = QRCodeHelpers.DefaultPPM;
    public bool ForceUTF8 { get; set; } = QRCodeHelpers.DefaultForceUTF8;
    public bool UTF8BOM { get; set; } = QRCodeHelpers.DefaultUTF8BOM;
    public int RequestedVersion { get; set; } = QRCodeHelpers.DefaultRequestedVersion;
    public QRCodeGenerator.ECCLevel ECC { get; set; } = QRCodeHelpers.DefaultECC;
    public QRCodeGenerator.EciMode EciMode { get; set; } = QRCodeHelpers.DefaultEciMode;
    public Stream? Logo { get; set; }
}
