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
    public class HospitalStatusViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public HospitalStatusViewModel()
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (Data.EpidemicIndicators == null)
                {
                    Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                    Data.EpidemicIndicators.AddRange(await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));

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
                TileHelper.SetDataTile("NewHospitalization", ChartType.BarAndLine, false, false, true, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("HospitalizedPatients", ChartType.Area, false, false, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewHospitalization", ChartType.Area, true, false, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewIntensiveCarePatients", ChartType.BarAndLine, false, false, true, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("IntensiveCarePatients", ChartType.Area, false, false, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewIntensiveCarePatients", ChartType.Area, true, false, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewDeceasedPersons", ChartType.BarAndLine, false, false, true, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("DeceasedPersons", ChartType.Area, false, true, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewDeceasedPersons", ChartType.Area, true, false, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewReturnHome", ChartType.BarAndLine, false, false, true, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewReturnHome", ChartType.Area, true, false, false, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("OccupationRate", ChartType.Area, false, false, true, true, 2, typeof(EpidemicIndicator))
            };

            List<Task> tasks = new();

            foreach (DataTile dataTile in DataTiles)
                tasks.Add(Task.Factory.StartNew(() => dataTile.ChartIndicators = TileHelper.GetChartIndicators(dataTile)));

            await Task.WhenAll(tasks);
        }
    }
}
