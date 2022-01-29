using System.Collections.Generic;
using System.Threading.Tasks;

using Covid19Dashboard.Core;
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
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (Data.VaccinationIndicators == null)
                {
                    Data.VaccinationIndicators = await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                    Data.VaccinationIndicators.AddRange(await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));

                    await UpdateDataTilesAsync();
                }
                else
                    await UpdateDataTilesAsync();
            });
        }

        public async Task UpdateDataTilesAsync()
        {
            DataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("NewFirstDoses", ChartType.BarAndLine, false, false, true, false, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("NewFirstDoses", ChartType.Area, true, false, false, true, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("TotalFirstDoses", ChartType.Area, false, false, false, false, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("NewCompleteVaccinations", ChartType.BarAndLine, false, false, true, false, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("NewCompleteVaccinations", ChartType.Area, true, false, false, true, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("TotalCompleteVaccinations", ChartType.Area, false, false, true, false, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("NewFirstBoosterDoses", ChartType.BarAndLine, false, false, true, false, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("NewFirstBoosterDoses", ChartType.Area, true, false, true, true, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("TotalFirstBoosterDoses", ChartType.Area, false, false, true, false, 0, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("FirstDosesCoverage", ChartType.Area, false, false, true, false, 2, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("CompleteVaccinationsCoverage", ChartType.Area, false, false, true, false, 2, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("FirstBoosterDosesCoverage", ChartType.Area, false, false, true, false, 2, typeof(VaccinationIndicator))
            };

            List<Task> tasks = new();

            foreach (DataTile dataTile in DataTiles)
                tasks.Add(Task.Factory.StartNew(() => dataTile.ChartIndicators = TileHelper.GetChartIndicators(dataTile)));

            await Task.WhenAll(tasks);
        }
    }
}
