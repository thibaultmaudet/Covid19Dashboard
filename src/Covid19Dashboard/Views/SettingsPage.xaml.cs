
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsViewModel ViewModel { get; } = new SettingsViewModel();

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ComboBox).SelectedItem as ComboBoxItem).Tag != null)
                ViewModel.SwitchThemeCommand.Execute((ElementTheme)((sender as ComboBox).SelectedItem as ComboBoxItem).Tag);
        }
    }
}
