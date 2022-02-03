using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using Windows.Storage;

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

        }

        public async Task UpdateDataTilesAsync()
        {
            Data.IsLoading = true;

            if (Data.EpidemicIndicators == null)
            {
                Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                Data.EpidemicIndicators.AddRange(await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));

                Data.Departments = EpidemicDataHelper.GetDepartments();
            }

            DataTiles = new();

            List<Task> tasks = new();

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("DailyConfirmedNewCases", false, true, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("DailyConfirmedNewCases", ChartType.Bar, false, false, true, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("DailyConfirmedNewCases", ChartType.Line, true, true, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("PositiveCases", false, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("PositiveCases", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("PositiveCases", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("IncidenceRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("IncidenceRate", ChartType.Area, false, 2, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("PositivityRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("PositivityRate", ChartType.Area, false, 2, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("ReproductionRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("ReproductionRate", ChartType.Area, false, 2, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            await Task.WhenAll(tasks);

            Data.IsLoading = false;
        }
    }
}
