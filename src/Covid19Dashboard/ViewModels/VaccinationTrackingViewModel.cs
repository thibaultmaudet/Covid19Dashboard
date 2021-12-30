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
    public class VaccinationTrackingViewModel : ObservableObject
    {
        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public VaccinationTrackingViewModel()
        {
            if (EpidemicDataHelper.EpidemicIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    EpidemicDataHelper.VaccinationIndicators = await EpidemicDataService.GetVaccinationIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path);

                    SetDataTiles();
                });
            }
            else
                SetDataTiles();

        }

        private void SetDataTiles()
        {
            EpidemicDataHelper.IndicatorType = typeof(VaccinationIndicator);

            DataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewFirstDoses"), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewFirstDoses"), Property = "NewFirstDoses" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewFirstDoses", true, 0), Digits = 0, Evolution = EpidemicDataHelper.GetEvolution("NewFirstDoses", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewFirstDoses"), Property = "NewFirstDoses" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("TotalFirstDoses"), LastUpdate = EpidemicDataHelper.GetLastUpdate("TotalFirstDoses"), Property = "TotalFirstDoses" },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewCompleteVaccinations"), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewCompleteVaccinations"), Property = "NewCompleteVaccinations" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewCompleteVaccinations", true, 0), Digits = 0, Evolution = EpidemicDataHelper.GetEvolution("NewCompleteVaccinations"), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewCompleteVaccinations"), Property = "NewCompleteVaccinations" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("TotalCompleteVaccinations"), LastUpdate = EpidemicDataHelper.GetLastUpdate("TotalCompleteVaccinations"), Property = "TotalCompleteVaccinations" },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewFirstBoosterDoses"), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewFirstBoosterDoses"), Property = "NewFirstBoosterDoses" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("NewFirstBoosterDoses", true, 0), Digits = 0, Evolution = EpidemicDataHelper.GetEvolution("NewFirstBoosterDoses"), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("NewFirstBoosterDoses"), Property = "NewFirstBoosterDoses" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("TotalFirstBoosterDoses"), LastUpdate = EpidemicDataHelper.GetLastUpdate("TotalFirstBoosterDoses"), Property = "TotalFirstBoosterDoses" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("FirstDosesCoverage", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("FirstDosesCoverage"), Property = "FirstDosesCoverage" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("CompleteVaccinationsCoverage", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("CompleteVaccinationsCoverage"), Property = "CompleteVaccinationsCoverage" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("FirstBoosterDosesCoverage", 2), Digits = 2, LastUpdate = EpidemicDataHelper.GetLastUpdate("FirstBoosterDosesCoverage"), Property = "FirstBoosterDosesCoverage" }
            };
        }
    }
}
