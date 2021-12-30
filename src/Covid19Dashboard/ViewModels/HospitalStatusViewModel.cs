using System.Collections.Generic;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Models;
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
            EpidemicDataHelper.IndicatorType = typeof(EpidemicIndicator);

            DataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewHospitalization"), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewHospitalization"), Property = "NewHospitalization" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("HospitalizedPatients"), Evolution = EpidemicDataHelper.GetEvolution("HospitalizedPatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("HospitalizedPatients"), Property = "HospitalizedPatients" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewHospitalization", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewHospitalization", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewHospitalization"), Property = "NewHospitalization" },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewIntensiveCarePatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewIntensiveCarePatients"), Property = "NewIntensiveCarePatients" },               
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IntensiveCarePatients"), Evolution = EpidemicDataHelper.GetEvolution("IntensiveCarePatients"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IntensiveCarePatients"), Property = "IntensiveCarePatients" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewIntensiveCarePatients", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewIntensiveCarePatients", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewIntensiveCarePatients"), Property = "NewIntensiveCarePatients" },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewDeceasedPersons"), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewDeceasedPersons"), Property = "NewDeceasedPersons" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewDeceasedPersons", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewDeceasedPersons", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewDeceasedPersons"), Property = "NewDeceasedPersons" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("DeceasedPersons"), LastUpdate = EpidemicDataHelper.GetLastUpdate("DeceasedPersons"), Property = "DeceasedPersons" },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewReturnHome"), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewReturnHome"), Property = "NewReturnHome" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewReturnHome", true, 0), Evolution = EpidemicDataHelper.GetEvolution("NewReturnHome", true), Digits = 2, IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewReturnHome"), Property = "NewReturnHome" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("OccupationRate", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("OccupationRate"), Property = "OccupationRate" }
            };
        }
    }
}
