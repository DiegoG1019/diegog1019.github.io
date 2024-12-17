using DiegoG.WebTools.Pages;
using System;

namespace DiegoG.WebTools.Services;

public sealed class LanguageItems
{
    public required string LanguageName { get; init; }
    public required string AboutMeUri { get; init; }
    public required string AboutMe { get; init; }
    public required string CVUri { get; init; }
    public required string Home { get; init; }
    public required string QRCodeGen { get; init; }
    public required string SiteTitle { get; init; }
    public required string ShortSiteTitle { get; init; }
    public required string QRContent { get; init; }
    public required string QRPPM { get; init; }
    public required string QRECC { get; init; }
    public required string QRECIMode { get; init; }
    public required string QRForceUTF8 { get; init; }
    public required string QRUTF8Bom { get; init; }
    public required string QRRequestedVersion { get; init; }
    public required string SubmitButton { get; init; }
    public required string NotFoundTitle { get; init; }
    public required string NotFoundMessage { get; init; }
}

public static class AvailableLanguages
{
    public static LanguageItems English { get; } = new LanguageItems()
    {
        LanguageName = "en",
        AboutMeUri = "https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/README.md",
        AboutMe = "About Me",
        CVUri = "/Diego CV - ENGLISH - DEC24.pdf",
        Home = "Home",
        QRCodeGen = "QR-Code Generator",
        SiteTitle = "Dev Engr. DiegoG & Tools",
        ShortSiteTitle = "Dev DiegoG",
        QRContent = "Content: ",
        QRPPM = "PPM: ",
        QRECC = "ECC: ",
        QRECIMode = "Eci Mode: ",
        QRForceUTF8 = "Force UTF8? ",
        QRUTF8Bom = "Include BOM in UTF-8? ",
        QRRequestedVersion = "QR Code Version: ",
        SubmitButton = "Submit",
        NotFoundTitle = "Not Found",
        NotFoundMessage = "Sorry, there's nothing at this address"
    };
}
