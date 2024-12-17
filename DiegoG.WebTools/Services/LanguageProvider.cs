using System.ComponentModel;

namespace DiegoG.WebTools.Services;

public sealed class LanguageProvider : INotifyPropertyChanged
{
    private static readonly PropertyChangedEventArgs changedArgs = new(nameof(CurrentLanguage));

    private LanguageItems _currentLanguage = AvailableLanguages.English;
    public LanguageItems CurrentLanguage
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
