using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class EpidemiologicalStatusViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private ObservableCollection<DataTile> dataTiles;

        public ObservableCollection<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public EpidemiologicalStatusViewModel()
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (Data.EpidemicIndicators == null)
                {
                    Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);

                    await UpdateDataTilesAsync();
                }
                else
                    await UpdateDataTilesAsync();
            });
        }

        public async Task UpdateDataTilesAsync()
        {
            DataTiles = new()
            {
                TileHelper.SetDataTile("DailyConfirmedNewCases", ChartType.BarAndLine, false, true, true, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("PositiveCases", ChartType.BarAndLine, false, false, true, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("IncidenceRate", ChartType.Area, false, false, false, true, 2, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("PositivityRate", ChartType.Area, false, false, false, true, 2, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("ReproductionRate", ChartType.Area, false, false, false, true, 2, typeof(EpidemicIndicator))
            };

            List<Task> tasks = new();

            foreach (DataTile dataTile in DataTiles)
                tasks.Add(Task.Factory.StartNew(() => dataTile.ChartIndicators = TileHelper.GetChartIndicators(dataTile)));

            await Task.WhenAll(tasks);
        }
    }
}
