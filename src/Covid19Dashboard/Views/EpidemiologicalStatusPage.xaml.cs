using System;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class EpidemiologicalStatusPage : Page
    {
        public EpidemiologicalStatusViewModel ViewModel { get; } = new EpidemiologicalStatusViewModel();

        public EpidemiologicalStatusPage()
        {
            InitializeComponent();
        }

        private void TileControl_SeeMoreDetailsClick(object sender, EventArgs e)
        {
            if ((sender as HyperlinkButton).Tag is DataTile dataTile)
                Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = dataTile.ChartType, ChartIndicators = EpidemicDataHelper.GetValuesForChart(dataTile.Property, dataTile.IsAverage, dataTile.Digits) });
        }
    }
}
