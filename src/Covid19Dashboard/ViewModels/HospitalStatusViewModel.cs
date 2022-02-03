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
    public class HospitalStatusViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private ObservableCollection<DataTile> dataTiles;

        public ObservableCollection<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public HospitalStatusViewModel()
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
                DataTile dataTile = TileHelper.SetDataTile("NewHospitalization", false, false, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewHospitalization", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewHospitalization", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("HospitalizedPatients", false, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("HospitalizedPatients", ChartType.Area, false, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewHospitalization", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewHospitalization", ChartType.Area, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewIntensiveCarePatients", false, false, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewIntensiveCarePatients", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewIntensiveCarePatients", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("IntensiveCarePatients", false, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("IntensiveCarePatients", ChartType.Area, false, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewIntensiveCarePatients", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewIntensiveCarePatients", ChartType.Area, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewDeceasedPersons", false, false, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewDeceasedPersons", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewDeceasedPersons", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("DeceasedPersons", false, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("DeceasedPersons", ChartType.Area, false, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewDeceasedPersons", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewDeceasedPersons", ChartType.Area, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewReturnHome", false, false, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewReturnHome", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewReturnHome", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewReturnHome", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewReturnHome", ChartType.Area, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("OccupationRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("OccupationRate", ChartType.Area, true, 0, typeof(EpidemicIndicator)));

                DataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            await Task.WhenAll(tasks);

            Data.IsLoading = false;
        }
    }
}
