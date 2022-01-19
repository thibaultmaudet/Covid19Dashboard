using System.Collections.Generic;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.UI.Core;

namespace Covid19Dashboard.ViewModels
{
    public class EpidemiologicalStatusViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public EpidemiologicalStatusViewModel()
        {
            if (Data.EpidemicIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);

                    UpdateDataTiles();
                });
            }
            else
                UpdateDataTiles();

        }

        public void UpdateDataTiles()
        {
            EpidemicDataHelper.IndicatorType = typeof(EpidemicIndicator);

            DataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("DailyConfirmedNewCases", ChartType.Bar, false, true, true, 0),
                TileHelper.SetDataTile("PositiveCases", ChartType.Bar, false, false, true, 0),
                TileHelper.SetDataTile("PositiveCases", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("IncidenceRate", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("PositivityRate", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("ReproductionRate", ChartType.Area, false, false, true, 2)
            };
        }
    }
}
