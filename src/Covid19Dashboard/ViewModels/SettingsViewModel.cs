using System.Windows.Input;

using Covid19Dashboard.Helpers;
using Covid19Dashboard.Services;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace Covid19Dashboard.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {

        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { SetProperty(ref _elementTheme, value); }
        }

        public int ThemeIndex
        {
            get
            {
                if (ElementTheme == ElementTheme.Dark) return 0;
                if (ElementTheme == ElementTheme.Light) return 1;
                if (ElementTheme == ElementTheme.Default) return 2;

                return 2;
            }
        }

        public string Version
        {
            get
            {
                PackageVersion version = Package.Current.Id.Version;

                return string.Format("{0} {1}.{2}.{3}.{4}", "Settings_Version".GetLocalized(), version.Major, version.Minor, version.Build, version.Revision);
            }
        }

        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<ElementTheme>(
                        async (param) =>
                        {
                            ElementTheme = param;
                            await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }

        public SettingsViewModel()
        {
        }
    }
}
