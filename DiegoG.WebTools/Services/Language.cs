using DiegoG.WebTools.Pages;
using System;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;

namespace DiegoG.WebTools.Services;

public sealed class Language
{
    public required string LanguageName { get; set; }
    public required string LanguageCode { get; init; }
    public required HashSet<string>? AlternateLanguageCodes { get; init; }
    public required string AboutMeUri { get; init; }
    public required string AboutMe { get; init; }
    public required string WelcomeMessage { get; init; }
    public required string CVUri { get; init; }
    public required string Home { get; init; }
    public required string QRCodeGen { get; init; }
    public required string MyWork { get; init; }
    public required string MyGigs { get; init; }
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
    public required string QRLogoOnCodeFeature { get; init; }
    public required string ContactMe { get; init; }
    public required string ContactMeans { get; init; }
    public required string AveragePriceRange { get; init; }
    public required string AveragePriceRangeDisclaimer { get; init; }
    public required string RegexMatch { get; init; }
    public required string RegexReplace { get; init; }
    public required string RegexSplit { get; init; }
    public required string RegexPage { get; init; }
    public required string RegexExpression { get; init; }
    public required string RegexResult { get; init; }
    public required string RegexTarget { get; init; }
    public required string RegexNoResult { get; init; }
}

public static class AvailableLanguages
{
    static AvailableLanguages()
    {
        Español = new Language()
        {
            LanguageName = "Español",
            LanguageCode = "esp",
            AlternateLanguageCodes = ["spa"],
            AboutMeUri = "https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/README-spanish.md",
            WelcomeMessage = "Bienvenido a mi sitio web! Desarrollado enteramente por mi con C#, Blazor, CSS, HTML y JavaScript",
            AboutMe = "Sobre Mí",
            CVUri = "/Diego CV - ESPAÑOL - DEC24.pdf",
            Home = "Principal",
            QRCodeGen = "Generador de QRs",
            SiteTitle = "Ingr. Dev DiegoG",
            ShortSiteTitle = "Dev DiegoG",
            QRContent = "Contenido: ",
            QRPPM = "Píxeles por Módulo: ",
            QRECC = "ECC: ",
            QRECIMode = "Modo Eci: ",
            QRForceUTF8 = "Forzar UTF-8? ",
            QRUTF8Bom = "Incluir el BOM en UTF-8? ",
            QRRequestedVersion = "Version de Código QR: ",
            SubmitButton = "Generar",
            NotFoundTitle = "No Encontrado",
            NotFoundMessage = "Lo siento, no hay nada en esta dirección",
            ContactMe = "Contáctame!",
            ContactMeans = "Métodos de Contacto",
            MyWork = "Mi Portafolio",
            MyGigs = "Mis Servicios",
            AveragePriceRange = "Rango de Precio Promedio",
            AveragePriceRangeDisclaimer = "El precio puede variar según la aplicación y necesidades. Contácteme para mas información.",

            QRLogoOnCodeFeature = "Pronto podrás incluir una imagen en tu código QR!",

            RegexMatch = "Encontrar",
            RegexReplace = "Reemplazar",
            RegexSplit = "Separar",
            RegexPage = "Expresiones Regulares",
            RegexExpression = "Expresión Regular",
            RegexTarget = "Texto a Probar",
            RegexResult = "Resultado",
            RegexNoResult = "Sin resultados"
        };

        English = new Language()
        {
            LanguageName = "English",
            LanguageCode = "eng",
            AlternateLanguageCodes = null,
            AboutMeUri = "https://raw.githubusercontent.com/DiegoG1019/DiegoG1019/refs/heads/main/README.md",
            WelcomeMessage = "Welcome to my website! Developed entirely by me with C#, Blazor, CSS, HTML and JavaScript",
            AboutMe = "About Me",
            CVUri = "/Diego CV - ENGLISH - DEC24.pdf",
            Home = "Home",
            QRCodeGen = "QR-Code Generator",
            SiteTitle = "Dev Engr. DiegoG & Tools",
            ShortSiteTitle = "Dev DiegoG",
            QRContent = "Content: ",
            QRPPM = "Pixels per Module: ",
            QRECC = "ECC: ",
            QRECIMode = "Eci Mode: ",
            QRForceUTF8 = "Force UTF8? ",
            QRUTF8Bom = "Include BOM in UTF-8? ",
            QRRequestedVersion = "QR Code Version: ",
            SubmitButton = "Submit",
            NotFoundTitle = "Not Found",
            NotFoundMessage = "Sorry, there's nothing at this address",
            ContactMe = "Contact Me!",
            ContactMeans = "Contact Means",
            MyWork = "My Portfolio",
            MyGigs = "My Services",
            AveragePriceRange = "Average Price Range",
            AveragePriceRangeDisclaimer = "Price may vary depending on application complexity. Contact me for further details.",

            QRLogoOnCodeFeature = "Logos on the QR code coming soon!",

            RegexMatch = "Match",
            RegexReplace = "Replace",
            RegexSplit = "Split",
            RegexPage = "Regular Expressions",
            RegexExpression = "Regular Expression",
            RegexTarget = "Target String",
            RegexResult = "Results",
            RegexNoResult = "No matches"
        };

        var langs = typeof(AvailableLanguages).GetProperties()
                                                  .Where(x => x.PropertyType == typeof(Language))
                                                  .Select(x => x.GetValue(null))
                                                  .Cast<Language>();
        Dictionary<string, Language> langDict = [];
        foreach (var lang in langs)
        {
            langDict.Add(lang.LanguageCode, lang);
            if (lang.AlternateLanguageCodes is not null and { Count: > 0 })
                foreach (var k in lang.AlternateLanguageCodes)
                    langDict.Add(k, lang);
        }

        Languages = langDict.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    public static FrozenDictionary<string, Language> Languages { get; }

    public static Language Español { get; }

    public static Language English { get; }
}
