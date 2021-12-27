using System;
using System.Collections.Generic;
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
            if (App.EpidemicIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    App.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicators(ApplicationData.Current.TemporaryFolder.Path);

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
                new DataTile() { Data = EpidemicDataHelper.GetDailyConfirmedNewCasesValue(App.EpidemicIndicators), Description = "NewCaseDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetDailyConfirmedNewCasesLastUpdate(App.EpidemicIndicators), Tag = "NewCase", Title = "NewCaseTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetPositiveCases(App.EpidemicIndicators), Description = "PositiveCasesDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetPositiveCasesLastUpdate(App.EpidemicIndicators), Tag = "PositiveCases", Title = "PositiveCasesTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetPositiveConfirmedNewCasesWeeklyAverage(App.EpidemicIndicators), Description = "PositiveCasesWeeklyAverageDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetPositiveCasesLastUpdate(App.EpidemicIndicators), Tag = "PositiveCasesWeeklyAverage", Title = "PositiveCasesWeeklyAverageTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetIncidenceRate(App.EpidemicIndicators), Description = "IncidenceRateDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetIncidenceRateLastUpdate(App.EpidemicIndicators), Tag = "IncidenceRate", Title = "IncidenceRateTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetPositivityRate(App.EpidemicIndicators), Description = "PositivityRateDescription".GetLocalized(), LastUpdate
                 = EpidemicDataHelper.GetPositiveCasesLastUpdate(App.EpidemicIndicators), Tag = "PositivityRate", Title = "PositivityRateTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetReproductionRate(App.EpidemicIndicators), Description = "ReproductionRateDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetReproductionRateLastUpdate(App.EpidemicIndicators), Tag = "ReproductionRate", Title = "ReproductionRateTitle".GetLocalized() },
            };
        }
    }
}
