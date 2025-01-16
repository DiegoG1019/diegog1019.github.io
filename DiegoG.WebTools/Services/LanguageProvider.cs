using System.ComponentModel;
using System.Globalization;

namespace DiegoG.WebTools.Services;

public sealed class LanguageProvider : INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs changedArgs = new(nameof(CurrentLanguage));

    public LanguageProvider()
    {
        var langcode = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;
        if (AvailableLanguages.Languages.TryGetValue(langcode, out var lang))
            _currentLanguage = lang;
        else
        {
            Console.WriteLine($"Could not find a language match for code {langcode}; falling back to English");
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

    public event PropertyChangedEventHandler? PropertyChanged;
}
