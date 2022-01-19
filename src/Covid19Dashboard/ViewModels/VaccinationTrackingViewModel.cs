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
    public class VaccinationTrackingViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public VaccinationTrackingViewModel()
        {
            if (Data.EpidemicIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Data.VaccinationIndicators = await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                    Data.VaccinationIndicators.AddRange(await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));

                    UpdateDataTiles();
                });
            }
            else
                UpdateDataTiles();

        }

        public void UpdateDataTiles()
        {
            EpidemicDataHelper.IndicatorType = typeof(VaccinationIndicator);

            DataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("NewFirstDoses", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("NewFirstDoses", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("TotalFirstDoses", ChartType.Area, false, false, false, 0),
                TileHelper.SetDataTile("NewCompleteVaccinations", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("NewCompleteVaccinations", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("TotalCompleteVaccinations", ChartType.Area, false, false, true, 0),
                TileHelper.SetDataTile("NewFirstBoosterDoses", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("NewFirstBoosterDoses", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("TotalFirstBoosterDoses", ChartType.Area, false, false, true, 0),
                TileHelper.SetDataTile("FirstDosesCoverage", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("CompleteVaccinationsCoverage", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("FirstBoosterDosesCoverage", ChartType.Area, false, false, true, 2)
            };
        }
    }
}
