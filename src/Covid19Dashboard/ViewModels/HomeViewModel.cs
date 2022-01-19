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
            if (Data.EpidemicIndicators == null || Data.VaccinationIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);
                    Data.EpidemicIndicators.AddRange(await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.Department));

                    Data.Departments = EpidemicDataHelper.GetDepartments();

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
            EpidemicDataHelper.IndicatorType = typeof(EpidemicIndicator);

            EpidemiologyDataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("DailyConfirmedNewCases", ChartType.Bar, false, true, true, 0),
                TileHelper.SetDataTile("IncidenceRate", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("PositivityRate", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("ReproductionRate", ChartType.Area, false, false, true, 2)
            };

            HospitalDataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("HospitalizedPatients", ChartType.Bar, false, true, false, 0),
                TileHelper.SetDataTile("IntensiveCarePatients", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("DeceasedPersons", ChartType.Bar, false, true, false, 0),
                TileHelper.SetDataTile("NewHospitalization", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("NewIntensiveCarePatients", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("NewDeceasedPersons", ChartType.Area, true, false, true, 0)
            };

            EpidemicDataHelper.IndicatorType = typeof(VaccinationIndicator);

            VaccinationDataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("FirstDosesCoverage", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("CompleteVaccinationsCoverage", ChartType.Area, false, false, true, 2),
                TileHelper.SetDataTile("FirstBoosterDosesCoverage", ChartType.Area, false, false, true, 2)
            };
        }
    }
}
