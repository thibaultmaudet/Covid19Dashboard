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
    public class HomeViewModel : ObservableObject
    {
        private List<DataTile> epidemiologyDataTiles;
        private List<DataTile> hospitalDataTiles;

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

        public HomeViewModel()
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
            EpidemiologyDataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("DailyConfirmedNewCases"), Description = "NewCaseDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("DailyConfirmedNewCases"), Property = "DailyConfirmedNewCases", Title = "NewCaseTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("IncidenceRate", 2), Description = "IncidenceRateDescription".GetLocalized(), Digits = 2, DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("IncidenceRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("IncidenceRate"), Property = "IncidenceRate", Title = "IncidenceRateTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("PositivityRate", 2), Description = "PositivityRateDescription".GetLocalized(), Digits = 2, DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("PositivityRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("PositivityRate"), Property = "PositivityRate", Title = "PositivityRateTitle".GetLocalized() },
                new DataTile() { ChartType = ChartType.Area, Data = EpidemicDataHelper.GetValue("ReproductionRate", 2), Description = "ReproductionRateDescription".GetLocalized(), Digits = 2, DisplayEvolution = true, Evolution = EpidemicDataHelper.GetEvolution("ReproductionRate"), LastUpdate = EpidemicDataHelper.GetLastUpdate("ReproductionRate"), Property = "ReproductionRate", Title = "ReproductionRateTitle".GetLocalized() }
            };

            HospitalDataTiles = new List<DataTile>
            {
                new DataTile() { ChartType = ChartType.Bar, Data = EpidemicDataHelper.GetValue("NewHospitalization"), Description = "NewHospitalizationDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetLastUpdate("NewHospitalization"), Property = "NewHospitalization", Title = "NewHospitalizationTitle".GetLocalized() }
            };
        }
    }
}
