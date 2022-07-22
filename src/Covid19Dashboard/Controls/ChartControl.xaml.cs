using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Covid19Dashboard.Core.Enums;
using Covid19Dashboard.Core.Models;

using LiveChartsCore;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    public sealed partial class ChartControl : UserControl, INotifyPropertyChanged
    {
        private ISeries[] series;

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(List<ChartIndicators>), typeof(ChartControl), new PropertyMetadata(default(List<ChartIndicators>), OnChartIndicatorsPropertyChanged));

        public List<ChartIndicators> Source
        {
            get { return (List<ChartIndicators>)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public ISeries[] Series
        {
            get { return series; }
            set { SetProperty(ref series, value); }
        }

        public IEnumerable<ICartesianAxis> XAxes { get; set; }

        public ChartControl()
        {
            InitializeComponent();

            XAxes = new Axis[] { new Axis { Labeler = value => new DateTime((long)value).ToShortDateString(), UnitWidth = TimeSpan.FromDays(1).Ticks, MinStep = TimeSpan.FromDays(7).Ticks } };
        }

        private static void OnChartIndicatorsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartControl control = d as ChartControl;
            control.Series = control.SetChart();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return;

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private ISeries[] SetChart()
        {
            ISeries[] s = (ISeries[])Array.CreateInstance(typeof(ISeries), Source.Count);

            for (int i = 0; i <= Source.Count - 1; i++)
            {
                int tooltipIndex = i;

                if (Source[i].ChartType == ChartType.Bar)
                    new ISeries[] { new ColumnSeries<ChartIndicator> { TooltipLabelFormatter = (chartPoint) => $"{ Source[tooltipIndex].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = Source[i] } }.CopyTo(s, i);
                else if (Source[i].ChartType == ChartType.Area)
                    new ISeries[] { new StackedAreaSeries<ChartIndicator> { LineSmoothness = 1, TooltipLabelFormatter = (chartPoint) => $"{ Source[tooltipIndex].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = Source[i] } }.CopyTo(s, i);
                else if (Source[i].ChartType == ChartType.Line)
                    new ISeries[] { new LineSeries<ChartIndicator> { Fill = null, GeometryFill = null, GeometryStroke = null, LineSmoothness = 1, TooltipLabelFormatter = (chartPoint) => $"{Source[tooltipIndex].Name + Environment.NewLine + new DateTime((long)chartPoint.SecondaryValue).ToShortDateString() } : {chartPoint.PrimaryValue}", Values = Source[i] } }.CopyTo(s, i);
            }

            return s;
        }
    }
}
