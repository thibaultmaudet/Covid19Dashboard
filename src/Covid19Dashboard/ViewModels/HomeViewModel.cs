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
    public class HomeViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private ObservableCollection<DataTile> epidemiologyDataTiles;
        private ObservableCollection<DataTile> hospitalDataTiles;
        private ObservableCollection<DataTile> vaccinationDataTiles;

        public ObservableCollection<DataTile> EpidemiologyDataTiles
        {
            get { return epidemiologyDataTiles; }
            set { SetProperty(ref epidemiologyDataTiles, value); }
        }

        public ObservableCollection<DataTile> HospitalDataTiles
        {
            get { return hospitalDataTiles; }
            set { SetProperty(ref hospitalDataTiles, value); }
        }

        public ObservableCollection<DataTile> VaccinationDataTiles
        {
            get { return vaccinationDataTiles; }
            set { SetProperty(ref vaccinationDataTiles, value); }
        }

        public HomeViewModel()
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

            if (Data.VaccinationIndicators == null)
            {
                Data.VaccinationIndicators = await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                Data.VaccinationIndicators.AddRange(await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));
            }

            EpidemiologyDataTiles = new();
            HospitalDataTiles = new();
            VaccinationDataTiles = new();

            List<Task> tasks = new();

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            // Epidemilogy indicators
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("DailyConfirmedNewCases", false, true, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("DailyConfirmedNewCases", ChartType.Bar, false, false, true, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("DailyConfirmedNewCases", ChartType.Line, true, true, true, 0, typeof(EpidemicIndicator)));

                EpidemiologyDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("IncidenceRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("IncidenceRate", ChartType.Area, false, 2, typeof(EpidemicIndicator)));

                EpidemiologyDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("PositivityRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("PositivityRate", ChartType.Area, false, 2, typeof(EpidemicIndicator)));

                EpidemiologyDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("ReproductionRate", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("ReproductionRate", ChartType.Area, false, 2, typeof(EpidemicIndicator)));

                EpidemiologyDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            // Hospitalization indicators
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("HospitalizedPatients", false, true, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("HospitalizedPatients", ChartType.Line, false, false, true, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("IntensiveCarePatients", ChartType.Line, false, false, true, 0, typeof(EpidemicIndicator)));

                HospitalDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("IntensiveCarePatients", false, false, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("HospitalizedPatients", ChartType.Line, false, false, true, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("IntensiveCarePatients", ChartType.Line, false, false, true, 0, typeof(EpidemicIndicator)));

                HospitalDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("DeceasedPersons", false, true, false, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("DeceasedPersons", ChartType.Area, false, 0, typeof(EpidemicIndicator)));

                HospitalDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewHospitalization", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewHospitalization", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewHospitalization", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                HospitalDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewIntensiveCarePatients", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewIntensiveCarePatients", ChartType.Bar, false, 0, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewIntensiveCarePatients", ChartType.Line, true, true, 0, typeof(EpidemicIndicator)));

                HospitalDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewDeceasedPersons", true, false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("NewDeceasedPersons", ChartType.Area, true, 0, typeof(EpidemicIndicator)));

                HospitalDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            // Vaccincation indicators
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("FirstDosesCoverage", false, false, true, 2, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("CompleteVaccinationsCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstBoosterDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));

                VaccinationDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("CompleteVaccinationsCoverage", false, false, true, 2, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("CompleteVaccinationsCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstBoosterDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));

                VaccinationDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("FirstBoosterDosesCoverage", false, false, true, 2, typeof(VaccinationIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("CompleteVaccinationsCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartIndicators("FirstBoosterDosesCoverage", ChartType.Line, false, 2, typeof(VaccinationIndicator)));

                VaccinationDataTiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            await Task.WhenAll(tasks);

            Data.IsLoading = false;
        }
    }
}
