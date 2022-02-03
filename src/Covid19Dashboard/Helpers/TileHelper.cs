using System;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Models;

namespace Covid19Dashboard.Helpers
{
    public class TileHelper
    {
        public static DataTile SetDataTile(string property, bool isAverage, bool isNationalIndicator, bool withEvolution, int digits, Type indicatorType)
        {
            DataTile dataTile = new()
            {
                ChartIndicators = new(),
                Data = EpidemicDataHelper.GetValue(property, isAverage, isNationalIndicator, digits, indicatorType),
                IndicatorType = indicatorType,
                IsAverage = isAverage,
                LastUpdate = EpidemicDataHelper.GetLastUpdate(property, isNationalIndicator, indicatorType),
                Property = property
            };

            if (withEvolution)
                dataTile.Evolution = EpidemicDataHelper.GetEvolution(property, isAverage, isNationalIndicator, indicatorType);

            return dataTile;
        }

        public static ChartIndicators GetChartIndicators(string property, ChartType chartType, bool isAverage, int digits, Type indicatorType)
        {
            return GetChartIndicators(property, chartType, isAverage, false, false, digits, indicatorType);
        }

        public static ChartIndicators GetChartIndicators(string property, ChartType chartType, bool isAverage, bool withAverage, int digits, Type indicatorType)
        {
            return GetChartIndicators(property, chartType, isAverage, withAverage, false, digits, indicatorType);
        }

        public static ChartIndicators GetChartIndicators(string property, ChartType chartType, bool isAverage, bool withAverage, bool isNationalIndicator, int digits, Type indicatorType)
        {
            ChartIndicators chartIndicators = new() { Name = !withAverage ? (!isAverage ? property.GetLocalized() : (property + "Average").GetLocalized()) : "SlidingAverage".GetLocalized(), ChartType = chartType };
            chartIndicators.Add(EpidemicDataHelper.GetValuesForChart(property, withAverage ? true : isAverage, isNationalIndicator, digits, indicatorType));

            return chartIndicators;
        }
    }
}
