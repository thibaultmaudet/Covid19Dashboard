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
    public class EpidemiologicalStatusViewModel : ObservableObject
    {
        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public EpidemiologicalStatusViewModel()
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
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("DailyConfirmedNewCases"), Description = "NewCaseDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("DailyConfirmedNewCases"), Property = "DailyConfirmedNewCases", Title = "NewCaseTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("PositiveCases"), Description = "PositiveCasesDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("PositiveCases"), Property = "PositiveCases", Title = "PositiveCasesTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("PositiveCases", true, 0), Description = "PositiveCasesWeeklyAverageDescription".GetLocalized(), DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("PositiveCases", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("PositiveCases"), Property = "PositiveCases", Title = "PositiveCasesWeeklyAverageTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IncidenceRate", 2), Description = "IncidenceRateDescription".GetLocalized(), Digits = 2, DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("IncidenceRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IncidenceRate"), Property = "IncidenceRate", Title = "IncidenceRateTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("PositivityRate", 2), Description = "PositivityRateDescription".GetLocalized(), Digits = 2, DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("PositivityRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("PositivityRate"), Property = "PositivityRate", Title = "PositivityRateTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("ReproductionRate", 2), Description = "ReproductionRateDescription".GetLocalized(), Digits = 2, DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("ReproductionRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("ReproductionRate"), Property = "ReproductionRate", Title = "ReproductionRateTitle".GetLocalized() }
            };
        }
    }
}
