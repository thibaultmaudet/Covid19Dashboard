using System;

using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class HospitalStatusPage : Page
    {
        public HospitalStatusViewModel ViewModel { get; } = new HospitalStatusViewModel();

        public HospitalStatusPage()
        {
            InitializeComponent();
        }

        private void TileControl_SeeMoreDetailsClick(object sender, EventArgs e)
        {
            if ((sender as HyperlinkButton).Tag is DataTile dataTile)
                Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = dataTile.ChartType, ChartIndicators = dataTile.ChartIndicators });
        }

        private async void FilterControl_FilterChanged(object sender, EventArgs e)
        {
            await ViewModel.UpdateDataTilesAsync();
        }
    }
}
