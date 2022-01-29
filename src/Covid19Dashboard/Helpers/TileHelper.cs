using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Models;

namespace Covid19Dashboard.Helpers
{
    public class TileHelper
    {
        public static DataTile SetDataTile(string property, ChartType chartType, bool isAverage, bool isNationalIndicator, bool withAverage, bool withEvolution, int digits, Type indicatorType)
        {
            DataTile dataTile = new()
            {
                ChartType = chartType,
                Data = EpidemicDataHelper.GetValue(property, isAverage, isNationalIndicator, digits, indicatorType),
                Digits = digits,
                IndicatorType = indicatorType,
                IsAverage = isAverage,
                IsNationalIndicator = isNationalIndicator,
                LastUpdate = EpidemicDataHelper.GetLastUpdate(property, isNationalIndicator, indicatorType),
                Property = property,
                WithAverage = withAverage
            };

            if (withEvolution)
                dataTile.Evolution = EpidemicDataHelper.GetEvolution(property, isAverage, isNationalIndicator, indicatorType);

            return dataTile;
        }

        public static List<ObservableCollection<ChartIndicator>> GetChartIndicators(DataTile dataTile)
        {
            List<ObservableCollection<ChartIndicator>> chartIndicators = new();
            chartIndicators.Add(EpidemicDataHelper.GetValuesForChart(!dataTile.IsAverage ? dataTile.Property.GetLocalized() : (dataTile.Property + "Average").GetLocalized(), dataTile.Property, dataTile.IsAverage, dataTile.IsNationalIndicator, dataTile.Digits, dataTile.IndicatorType));

            if (dataTile.WithAverage)
                chartIndicators.Add(EpidemicDataHelper.GetValuesForChart("SlidingAverage".GetLocalized(), dataTile.Property, true, dataTile.IsNationalIndicator, dataTile.Digits, dataTile.IndicatorType));

            return chartIndicators;
        }
    }
}
