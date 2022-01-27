using System;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Covid19Dashboard.Views
{
    public sealed partial class ChartPage : Page
    {
        public ChartViewModel ViewModel { get; } = new ChartViewModel();

        public ChartPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ChartParameter)
            {
                ViewModel.ChartType = (e.Parameter as ChartParameter).ChartType;

                if (ViewModel.ChartType == ChartType.Bar)
                    ViewModel.Series = new ISeries[] { new ColumnSeries<ChartIndicator> { Values = (e.Parameter as ChartParameter).ChartIndicators, TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}" } };
                else if (ViewModel.ChartType == ChartType.Area)
                    ViewModel.Series = new ISeries[] { new StackedAreaSeries<ChartIndicator> { Values = (e.Parameter as ChartParameter).ChartIndicators, TooltipLabelFormatter = (chartPoint) => $"{new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}" } };
            }
        }
    }
}
