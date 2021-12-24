using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.Views;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

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
            EpidemiologyDataTiles = new List<DataTile>
            {
                new DataTile() { Data = EpidemicDataHelper.GetDailyConfirmedNewCasesValue(App.EpidemicIndicators), Description = "NewCaseDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetDailyConfirmedNewCasesLastUpdate(App.EpidemicIndicators), Tag = "NewCase", Title = "NewCaseTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetIncidenceRate(App.EpidemicIndicators), Description = "IncidenceRateDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetIncidenceRateLastUpdate(App.EpidemicIndicators), Tag = "IncidenceRate", Title = "IncidenceRateTitle".GetLocalized() },
                new DataTile() { Data = EpidemicDataHelper.GetReproductionRate(App.EpidemicIndicators), Description = "ReproductionRateDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetReproductionRateLastUpdate(App.EpidemicIndicators), Tag = "ReproductionRate", Title = "ReproductionRateTitle".GetLocalized() },
            };

            HospitalDataTiles = new List<DataTile>
            {
                new DataTile() { Data = EpidemicDataHelper.GetNewHospitalization(App.EpidemicIndicators), Description = "NewHospitalizationDescription".GetLocalized(), LastUpdate = EpidemicDataHelper.GetNewHospitalizationLastUpdate(App.EpidemicIndicators), Tag = "NewHospitalization", Title = "NewHospitalizationTitle".GetLocalized() }
            };
        }
    }
}
