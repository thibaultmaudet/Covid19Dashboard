using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Models;

namespace Covid19Dashboard.Helpers
{
    public class TileHelper
    {
        public static DataTile SetDataTile(string property, ChartType chartType, bool isAverage, bool isNationalIndicator, bool withEvolution, int digits)
        {
            DataTile dataTile = new DataTile()
            {
                ChartType = chartType,
                Data = EpidemicDataHelper.GetValue(property, isAverage, isNationalIndicator, digits),
                Digits = digits,
                IsAverage = isAverage,
                IsNationalIndicator = isNationalIndicator,
                LastUpdate = EpidemicDataHelper.GetLastUpdate(property, isNationalIndicator),
                Property = property
            };

            if (withEvolution)
                dataTile.Evolution = EpidemicDataHelper.GetEvolution(property, isAverage, isNationalIndicator);

            return dataTile;
        }
    }
}
