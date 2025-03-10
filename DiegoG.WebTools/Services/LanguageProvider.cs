using System.ComponentModel;
using System.Globalization;
using System.Text.Json;

namespace DiegoG.WebTools.Services;

public static class LanguageExtensions
{
    public static string GetLocalizedString(this Dictionary<string, string> localized, Language currentLanguage) 
        => localized.TryGetValue(currentLanguage.LanguageCode, out var value) ? value : localized["eng"];
}

public sealed class LanguageProvider : INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs changedArgs = new(nameof(CurrentLanguage));

    public LanguageProvider(ILogger<LanguageProvider> logger)
    {
        var langcode = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
        logger.LogInformation("Trying to set language to {lang}", langcode);

        if (AvailableLanguages.Languages.TryGetValue(langcode, out var lang))
            _currentLanguage = lang;
        else
        {
            logger.LogWarning("Could not find a language match for code {lang}; falling back to English", langcode);
            _currentLanguage = AvailableLanguages.English;
        }
    }

    private Language _currentLanguage;
    public Language CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == _currentLanguage) return;
            _currentLanguage = value;
            PropertyChanged?.Invoke(this, changedArgs);
        }
    }

    public static NumberFormatInfo DefaultEnglishNumberFormatInfo { get; }
        = new NumberFormatInfo()
        {
            CurrencyDecimalDigits = 2,
            CurrencyDecimalSeparator = ".",
            CurrencyGroupSizes = [ 3 ],
            NumberGroupSizes = [ 3 ],
            PercentGroupSizes = [ 3 ],
            CurrencyGroupSeparator = ",",
            CurrencySymbol = "$",
            NaNSymbol = "NaN",
            CurrencyNegativePattern = 0,
            NumberNegativePattern = 1,
            PercentPositivePattern = 1,
            PercentNegativePattern = 1,
            NegativeInfinitySymbol = "-\u221E",
            NegativeSign = "-",
            NumberDecimalDigits = 2,
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ",",
            CurrencyPositivePattern = 0,
            PositiveInfinitySymbol = "\u221E",
            PositiveSign = "\u002B",
            PercentDecimalDigits = 2,
            PercentDecimalSeparator = ".",
            PercentGroupSeparator = ",",
            PercentSymbol = "%",
            PerMilleSymbol = "\u2030",
            NativeDigits = [ "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" ],
            DigitSubstitution = DigitShapes.None
        };

    public event PropertyChangedEventHandler? PropertyChanged;
}
