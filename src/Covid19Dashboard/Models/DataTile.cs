using System;
using System.Collections.Generic;

using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;

namespace Covid19Dashboard.Models
{
    public class DataTile
    {
        private readonly Lazy<List<ChartIndicators>> chartIndicators;

        public bool IsAverage { get; set; }

        public bool IsHomeTile { get; set; }

        public double Evolution { get; set; } = int.MaxValue;

        public List<ChartIndicators> ChartIndicators { get { return chartIndicators.Value; } }

        public string Data { get; set; }

        public string Description { get { return string.Format("{0} {1}", Data, (Property + (IsAverage ? "Average" : "") + "Description").GetLocalized()); } }

        public string LastUpdate { get; set; }

        public string Property { get; set; }

        public string Title { get { return (Property + (IsAverage ? "Average" : "") + "Title").GetLocalized(); } }

        public Type IndicatorType { get; set; }

        public Type MoreDetailPage { get; set; }

        public DataTile()
        {

        }

        public DataTile(List<DataIndicator> dataIndicators) : this(dataIndicators, false) { }

        public DataTile(List<DataIndicator> dataIndicators, bool isEvolutionChart)
        {
            chartIndicators = new(() =>
            {
                List<ChartIndicators> indicators = new();
                dataIndicators.ForEach(delegate (DataIndicator data) { indicators.Add(isEvolutionChart ? TileHelper.GetChartEvolutionIndicators(data.Property, data.ChartType, data.WithAverage, data.IndicatorType) : TileHelper.GetChartIndicators(data.Property, data.ChartType, data.IsAverage, data.WithAverage, data.IsNationalIndicator, data.Digits, data.IndicatorType)); });

                return indicators;
            });
        }
    }
}
