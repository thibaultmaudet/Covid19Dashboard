using System;
using System.Collections.Generic;

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

            if (e.Parameter is List<ChartIndicators>)
            {
                List<ChartIndicators> chartIndicators = (e.Parameter as List<ChartIndicators>);

                ViewModel.Series = (ISeries[])Array.CreateInstance(typeof(ISeries), chartIndicators.Count);

                for (int i = 0; i <= chartIndicators.Count - 1; i++)
                {
                    int tooltipIndex = i;

                    if (chartIndicators[i].ChartType == ChartType.Bar)
                        new ISeries[] { new ColumnSeries<ChartIndicator> { TooltipLabelFormatter = (chartPoint) => $"{ chartIndicators[tooltipIndex].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[i] } }.CopyTo(ViewModel.Series, i);
                    else if (chartIndicators[i].ChartType == ChartType.Area)
                        new ISeries[] { new StackedAreaSeries<ChartIndicator> { LineSmoothness = 1, TooltipLabelFormatter = (chartPoint) => $"{ chartIndicators[tooltipIndex].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[i] } }.CopyTo(ViewModel.Series, i);
                    else if (chartIndicators[i].ChartType == ChartType.Line)
                        new ISeries[] { new LineSeries<ChartIndicator> { Fill = null, GeometryFill = null, GeometryStroke = null, LineSmoothness = 1, TooltipLabelFormatter = (chartPoint) => $"{chartIndicators[tooltipIndex].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = chartIndicators[i] } }.CopyTo(ViewModel.Series, i);
                }
            }
        }
    }
}
