using System.Collections.Generic;
using System.Threading.Tasks;

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
    public class HomeViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private List<DataTile> epidemiologyDataTiles;
        private List<DataTile> hospitalDataTiles;
        private List<DataTile> vaccinationDataTiles;

        public List<DataTile> EpidemiologyDataTiles
        {
            get { return epidemiologyDataTiles; }
            set { SetProperty(ref epidemiologyDataTiles, value); }
        }

        public List<DataTile> HospitalDataTiles
        {
            get { return hospitalDataTiles; }
            set { SetProperty(ref hospitalDataTiles, value); }
        }

        public List<DataTile> VaccinationDataTiles
        {
            get { return vaccinationDataTiles; }
            set { SetProperty(ref vaccinationDataTiles, value); }
        }

        public HomeViewModel()
        {
            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                if (Data.EpidemicIndicators == null)
                {
                    Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                    Data.EpidemicIndicators.AddRange(await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));

                    Data.Departments = EpidemicDataHelper.GetDepartments();

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
            EpidemiologyDataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("DailyConfirmedNewCases", ChartType.BarAndLine, false, true, true, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("IncidenceRate", ChartType.Area, false, false, false, true, 2, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("PositivityRate", ChartType.Area, false, false, false, true, 2, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("ReproductionRate", ChartType.Area, false, false, false, true, 2, typeof(EpidemicIndicator))
            };

            HospitalDataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("HospitalizedPatients", ChartType.Area, false, true, false, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("IntensiveCarePatients", ChartType.Area, false, false, false, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("DeceasedPersons", ChartType.Area, false, true, false, false, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewHospitalization", ChartType.Area, true, false, true, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewIntensiveCarePatients", ChartType.Area, true, false, true, true, 0, typeof(EpidemicIndicator)),
                TileHelper.SetDataTile("NewDeceasedPersons", ChartType.Area, true, false, true, true, 0, typeof(EpidemicIndicator))
            };

            VaccinationDataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("FirstDosesCoverage", ChartType.Area, false, false, false, true, 2, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("CompleteVaccinationsCoverage", ChartType.Area, false, false, false, true, 2, typeof(VaccinationIndicator)),
                TileHelper.SetDataTile("FirstBoosterDosesCoverage", ChartType.Area, false, false, false, true, 2, typeof(VaccinationIndicator))
            };

            List<Task> tasks = new();

            foreach (DataTile dataTile in EpidemiologyDataTiles)
                tasks.Add(Task.Factory.StartNew(() => dataTile.ChartIndicators = TileHelper.GetChartIndicators(dataTile)));


            foreach (DataTile dataTile in HospitalDataTiles)
                tasks.Add(Task.Factory.StartNew(() => dataTile.ChartIndicators = TileHelper.GetChartIndicators(dataTile)));


            foreach (DataTile dataTile in VaccinationDataTiles)
                tasks.Add(Task.Factory.StartNew(() => dataTile.ChartIndicators = TileHelper.GetChartIndicators(dataTile)));

            await Task.WhenAll(tasks);
        }
    }
}
