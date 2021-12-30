using System.Collections.Generic;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Models;
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
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("DailyConfirmedNewCases"), LastUpdate = EpidemicDataHelper.GetLastUpdate("DailyConfirmedNewCases"), Property = "DailyConfirmedNewCases" },
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("PositiveCases"), LastUpdate = EpidemicDataHelper.GetLastUpdate("PositiveCases"), Property = "PositiveCases" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue<string>("PositiveCases", true, 0), Evolution = EpidemicDataHelper.GetEvolution("PositiveCases", true), IsAverage = true, LastUpdate = EpidemicDataHelper.GetLastUpdate("PositiveCases"), Property = "PositiveCases" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IncidenceRate", 2), Digits = 2, Evolution = EpidemicDataHelper.GetEvolution("IncidenceRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IncidenceRate"), Property = "IncidenceRate" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("PositivityRate", 2), Digits = 2, Evolution = EpidemicDataHelper.GetEvolution("PositivityRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("PositivityRate"), Property = "PositivityRate" },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("ReproductionRate", 2), Digits = 2, Evolution = EpidemicDataHelper.GetEvolution("ReproductionRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("ReproductionRate"), Property = "ReproductionRate" }
            };
        }
    }
}
