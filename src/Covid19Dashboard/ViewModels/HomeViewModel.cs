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
            if (EpidemicDataHelper.EpidemicIndicators == null || EpidemicDataHelper.VaccinationIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    EpidemicDataHelper.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicators(ApplicationData.Current.TemporaryFolder.Path);

                    EpidemicDataHelper.VaccinationIndicators = await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path);

                    SetDataTiles();
                });
            }
            else
                SetDataTiles();
            
        }

        private void SetDataTiles()
        {
            EpidemicDataHelper.IndicatorType = typeof(EpidemicIndicator);

            EpidemiologyDataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("DailyConfirmedNewCases"), LastUpdate = EpidemicDataHelper.GetLastUpdate("DailyConfirmedNewCases"), Property = "DailyConfirmedNewCases" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IncidenceRate", 2), Digits = 2, Evolution = EpidemicDataHelper.GetEvolution("IncidenceRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IncidenceRate"), Property = "IncidenceRate" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("PositivityRate", 2), Digits = 2, Evolution = EpidemicDataHelper.GetEvolution("PositivityRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("PositivityRate"), Property = "PositivityRate" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("ReproductionRate", 2), Digits = 2, Evolution = EpidemicDataHelper.GetEvolution("ReproductionRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("ReproductionRate"), Property = "ReproductionRate" }
            };

            HospitalDataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("HospitalizedPatients"),Evolution = EpidemicDataHelper.GetEvolution("HospitalizedPatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("HospitalizedPatients"), Property = "HospitalizedPatients" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IntensiveCarePatients"), Evolution = EpidemicDataHelper.GetEvolution("IntensiveCarePatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IntensiveCarePatients"), Property = "IntensiveCarePatients" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("DeceasedPersons"),LastUpdate = EpidemicDataHelper.GetLastUpdate("DeceasedPersons"), Property = "DeceasedPersons" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewHospitalization", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewHospitalization", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewHospitalization"), Property = "NewHospitalization" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewIntensiveCarePatients", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewIntensiveCarePatients", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewIntensiveCarePatients"), Property = "NewIntensiveCarePatients" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewDeceasedPersons", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewDeceasedPersons", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewDeceasedPersons"), Property = "NewDeceasedPersons" },
            };

            EpidemicDataHelper.IndicatorType = typeof(VaccinationIndicator);

            VaccinationDataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("FirstDosesCoverage", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("FirstDosesCoverage"), Property = "FirstDosesCoverage" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("CompleteVaccinationsCoverage", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("CompleteVaccinationsCoverage"), Property = "CompleteVaccinationsCoverage" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("FirstBoosterDosesCoverage", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("FirstBoosterDosesCoverage"), Property = "FirstBoosterDosesCoverage" }
            };
        }
    }
}
