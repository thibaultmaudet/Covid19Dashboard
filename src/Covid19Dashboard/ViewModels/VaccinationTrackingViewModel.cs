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
    public class VaccinationTrackingViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private ObservableCollection<DataTile> dataTiles;

        public ObservableCollection<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public VaccinationTrackingViewModel()
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
                DataTile dataTile = TileHelper.SetDataTile("NewFirstDoses", false, false, false, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewFirstDoses", ChartType.Bar, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewFirstDoses", ChartType.Line, true, true, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewFirstDoses", true, false, true, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewFirstDoses", ChartType.Area, true, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("TotalFirstDoses", false, false, true, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalFirstDoses", ChartType.Line, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalCompleteVaccinations", ChartType.Line, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalFirstBoosterDoses", ChartType.Line, false, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewCompleteVaccinations", false, false, false, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewCompleteVaccinations", ChartType.Bar, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewCompleteVaccinations", ChartType.Line, true, true, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewCompleteVaccinations", true, false, true, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewCompleteVaccinations", ChartType.Area, true, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("TotalCompleteVaccinations", false, false, true, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalFirstDoses", ChartType.Line, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalCompleteVaccinations", ChartType.Line, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalFirstBoosterDoses", ChartType.Line, false, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewFirstBoosterDoses", false, false, false, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewFirstBoosterDoses", ChartType.Bar, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewFirstBoosterDoses", ChartType.Line, true, true, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewFirstBoosterDoses", true, false, true, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewFirstBoosterDoses", ChartType.Area, true, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("TotalFirstBoosterDoses", false, false, true, 0, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalFirstDoses", ChartType.Line, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalCompleteVaccinations", ChartType.Line, false, 0, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("TotalFirstBoosterDoses", ChartType.Line, false, 0, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("FirstDosesCoverage", false, false, true, 2, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("CompleteVaccinationsCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstBoosterDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("CompleteVaccinationsCoverage", false, false, true, 2, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("CompleteVaccinationsCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstBoosterDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("FirstBoosterDosesCoverage", false, false, true, 2, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("CompleteVaccinationsCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstBoosterDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            await Task.WhenAll(tasks);

            Data.IsLoading = false;
        }
    }
}
