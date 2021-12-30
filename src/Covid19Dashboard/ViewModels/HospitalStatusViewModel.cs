using System.Collections.Generic;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.UI.Core;

namespace Covid19Dashboard.ViewModels
{
    public class HospitalStatusViewModel : ObservableObject
    {
        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public HospitalStatusViewModel()
        {
            if (EpidemicDataHelper.EpidemicIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    EpidemicDataHelper.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicators(ApplicationData.Current.TemporaryFolder.Path);

                    SetDataTiles();
                });
            }
            else
                SetDataTiles();

        }

        private void SetDataTiles()
        {
            DataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewHospitalization"), Description = "NewHospitalizationDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewHospitalization"), Property = "NewHospitalization", Title = "NewHospitalizationTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("HospitalizedPatients"), Description = "HospitalizedPatientsDescription".GetLocalized(), DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("HospitalizedPatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("HospitalizedPatients"), Property = "HospitalizedPatients", Title = "HospitalizedPatientsTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewHospitalization", true, 0), Description = "NewHospitalizationAverageDescription".GetLocalized(), DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("NewHospitalization", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewHospitalization"), Property = "NewHospitalization", Title = "NewHospitalizationAverageTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewIntensiveCarePatients"), Description = "NewIntensiveCarePatientsDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewIntensiveCarePatients"), Property = "NewIntensiveCarePatients", Title = "NewIntensiveCarePatientsTitle".GetLocalized() },               
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IntensiveCarePatients"), Description = "IntensiveCarePatientsDescription".GetLocalized(), DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("IntensiveCarePatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IntensiveCarePatients"), Property = "IntensiveCarePatients", Title = "IntensiveCarePatientsTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewIntensiveCarePatients", true, 0), Description = "NewIntensiveCarePatientsAverageDescription".GetLocalized(), DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("NewIntensiveCarePatients", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewIntensiveCarePatients"), Property = "NewIntensiveCarePatients", Title = "NewIntensiveCarePatientsAverageTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewDeceasedPersons"), Description = "NewDeceasedPersonsDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewDeceasedPersons"), Property = "NewDeceasedPersons", Title = "NewDeceasedPersonsTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewDeceasedPersons", true, 0), Description = "NewDeceasedPersonsAverageDescription".GetLocalized(), DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("NewDeceasedPersons", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewDeceasedPersons"), Property = "NewDeceasedPersons", Title = "NewDeceasedPersonsAverageTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("DeceasedPersons"), Description = "DeceasedPersonsDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("DeceasedPersons"), Property = "DeceasedPersons", Title = "DeceasedPersonsTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewReturnHome"), Description = "NewReturnHomeDescription".GetLocalized(), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewReturnHome"), Property = "NewReturnHome", Title = "NewReturnHomeTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewReturnHome", true, 0), Description = "NewReturnHomeAverageDescription".GetLocalized(), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewReturnHome"), Property = "NewReturnHome", Title = "NewReturnHomeAverageTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("OccupationRate", 2), Description = "OccupationRateDescription".GetLocalized(), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("OccupationRate"), Property = "OccupationRate", Title = "OccupationRateTitle".GetLocalized() }
            };
        }
    }
}
