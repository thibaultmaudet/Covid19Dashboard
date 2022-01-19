using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Covid19Dashboard.Core.Services
{
    public class EpidemicDataService
    {
        public static async Task<List<EpidemicIndicator>> GetEpedimicIndicatorsAsync(string filePath, Area area)
        {
            filePath = Path.Combine(filePath, Guid.NewGuid() + ".csv");
            
            // Download the summary of indicators for monitoring the COVID-19 epidemic csv file.
            WebClient webClient = new WebClient();
            if (area == Area.National)
                webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/f335f9ea-86e3-4ffa-9684-93c009d5e617", filePath);
            else if (area == Area.Department)
                webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/5c4e1452-3850-4b59-b11c-3dd51d7fb8b5", filePath);

            return (await CsvHelper.ToObject<List<EpidemicIndicator>>(filePath, ',')).OrderByDescending(x => x.Date).ToList();
        }

        public static async Task<List<VaccinationIndicator>> GetVaccinationIndicatorsAsync(string filePath, Area area)
        {
            filePath = Path.Combine(filePath, Guid.NewGuid() + ".csv");

            // Download the summary of indicators for monitoring the COVID-19 vaccination csv file.
            WebClient webClient = new WebClient();
            
            if (area == Area.National)
                webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/fa4ad329-14ec-4394-85a4-c5df33769dff", filePath);
            else if (area == Area.Department)
                webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/f77106ed-6e27-48cf-85b7-daad2b7fce1e", filePath);

            return (await CsvHelper.ToObject<List<VaccinationIndicator>>(filePath, ';')).OrderByDescending(x => x.Date).ToList();
        }
    }
}
