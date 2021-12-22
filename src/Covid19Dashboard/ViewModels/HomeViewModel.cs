using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;

namespace Covid19Dashboard.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private List<DataTile> epidemiologyDataTiles;
        private List<DataTile> hospitalDataTiles;

        private List<EpidemicIndicator> epidemicIndicators;

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

        public List<EpidemicIndicator> EpidemicIndicators
        {
            get { return epidemicIndicators; }
            set { SetProperty(ref epidemicIndicators, value); }
        }

        public HomeViewModel()
        {
            EpidemiologyDataTiles = new List<DataTile>();

            EpidemicIndicators = new List<EpidemicIndicator>();

            _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                EpidemicIndicators = await GetEpidemicIndicators();

                if (EpidemicIndicators.Count > 0)
                {
                    EpidemiologyDataTiles = new List<DataTile>
                    {
                        new DataTile() { Data = GetDailyConfirmedNewCasesValue(), Description = "Home_NewCaseDescription".GetLocalized(), LastUpdate = GetDailyConfirmedNewCasesLastUpdate(), Tag = "NewCase", Title = "Home_NewCaseTitle".GetLocalized() },
                        new DataTile() { Data = GetIncidenceRate(), Description = "Home_IncidenceRateDescription".GetLocalized(), LastUpdate = GetIncidenceRateLastUpdate(), Tag = "IncidenceRate", Title = "Home_IncidenceRateTitle".GetLocalized() },
                        new DataTile() { Data = GetReproductionRate(), Description = "Home_ReproductionRateDescription".GetLocalized(), LastUpdate = GetReproductionRateLastUpdate(), Tag = "ReproductionRate", Title = "Home_ReproductionRateTitle".GetLocalized() },
                    };

                    HospitalDataTiles = new List<DataTile>
                    {
                        new DataTile() { Data = GetNewHospitalization(), Description = "Home_NewHospitalizationDescription".GetLocalized(), LastUpdate = GetNewHospitalizationLastUpdate(), Tag = "NewHospitalization", Title = "Home_NewHospitalizationTitle".GetLocalized() }
                    };
                }
            });
        }

        public async Task<List<EpidemicIndicator>> GetEpidemicIndicators()
        {
            // Download the summary of indicators for monitoring the COVID-19 epidemic csv file (national).
            Uri source = new Uri("https://www.data.gouv.fr/fr/datasets/r/f335f9ea-86e3-4ffa-9684-93c009d5e617");

            StorageFile destinationFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync(Guid.NewGuid() + ".csv", CreationCollisionOption.GenerateUniqueName);

            DownloadOperation download = new BackgroundDownloader().CreateDownload(source, destinationFile);
            await download.StartAsync();

            List<EpidemicIndicator> epidemicIndicators = await CsvHelper.ToObject<List<EpidemicIndicator>>(destinationFile);
            return epidemicIndicators.OrderByDescending(x => x.Date).ToList();
        }

        private string GetDailyConfirmedNewCasesValue()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
            {
                int? hospitalization = EpidemicIndicators.FirstOrDefault(x => x.DailyConfirmedNewCases.HasValue).DailyConfirmedNewCases;

                return hospitalization.HasValue ? hospitalization.ToString() : "";
            }

            return "";
        }

        private string GetDailyConfirmedNewCasesLastUpdate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.First(x => x.DailyConfirmedNewCases.HasValue).Date.ToShortDateString();

            return "";
        }

        private string GetIncidenceRate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.FirstOrDefault(x => x.IncidenceRate.HasValue).IncidenceRate.Value.ToString("0.00");

            return "";
        }

        private string GetIncidenceRateLastUpdate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.First(x => x.IncidenceRate.HasValue).Date.ToShortDateString();

            return "";
        }

        private string GetNewHospitalization()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.FirstOrDefault(x => x.NewHospitalization.HasValue).NewHospitalization.Value.ToString();

            return "";
        }

        private string GetNewHospitalizationLastUpdate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.First(x => x.NewHospitalization.HasValue).Date.ToShortDateString();

            return "";
        }

        private string GetReproductionRate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.FirstOrDefault(x => x.ReproductionRate.HasValue).ReproductionRate.Value.ToString("0.00");

            return "";
        }

        private string GetReproductionRateLastUpdate()
        {
            if (EpidemicIndicators != null && EpidemicIndicators.Count > 0)
                return EpidemicIndicators.First(x => x.ReproductionRate.HasValue).Date.ToShortDateString();

            return "";
        }
    }
}
