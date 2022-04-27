using System;

using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class VaccinationTrackingPage : Page
    {
        public VaccinationTrackingViewModel ViewModel { get; } = new VaccinationTrackingViewModel();

        public VaccinationTrackingPage()
        {
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await ViewModel.UpdateDataTilesAsync(false);
        }

        private async void FilterControl_FilterChanged(object sender, EventArgs e)
        {
            await ViewModel.UpdateDataTilesAsync(true);
        }
    }
}
