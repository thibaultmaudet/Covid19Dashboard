using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                List<ObservableCollection<ChartIndicator>> chartIndicators = (e.Parameter as ChartParameter).ChartIndicators;

                ViewModel.ChartType = (e.Parameter as ChartParameter).ChartType;

                if (ViewModel.ChartType == ChartType.Bar)
                    ViewModel.Series = new ISeries[] { new ColumnSeries<ChartIndicator> { TooltipLabelFormatter = (chartPoint) => $"{ chartIndicators[0][0].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[0] } };
                else if (ViewModel.ChartType == ChartType.Area)
                    ViewModel.Series = new ISeries[] { new StackedAreaSeries<ChartIndicator> { LineSmoothness = 1, TooltipLabelFormatter = (chartPoint) => $"{ chartIndicators[0][0].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[0] } };
                else if (ViewModel.ChartType == ChartType.BarAndLine)
                    ViewModel.Series = new ISeries[] { new ColumnSeries<ChartIndicator> { TooltipLabelFormatter = (chartPoint) => $"{ chartIndicators[0][0].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[0] }, new LineSeries<ChartIndicator> { Fill = null, GeometryFill = null, GeometryStroke = null, LineSmoothness = 1, TooltipLabelFormatter = (chartPoint) => $"{ chartIndicators[1][0].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[1] } };
            }
        }
    }
}
