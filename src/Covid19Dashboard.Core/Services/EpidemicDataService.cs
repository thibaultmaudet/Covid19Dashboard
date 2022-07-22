using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Covid19Dashboard.Core.Enums;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;

namespace Covid19Dashboard.Core.Services
{
    public class EpidemicDataService
    {
        public static async Task<List<EpidemicIndicator>> GetEpedimicIndicatorsAsync(string filePath, Area area)
        {
            filePath = Path.Combine(filePath, Guid.NewGuid() + ".csv");

            // Download the summary of indicators for monitoring the COVID-19 epidemic csv file.
            WebClient webClient = new();
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
            WebClient webClient = new();

            if (area == Area.National)
                webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/e6fcd558-2ff6-487b-b624-e64a8c05bb07", filePath);
            else if (area == Area.Department)
                webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/64605b19-b3bd-4c26-9f30-e0c3915290d3", filePath);

            return (await CsvHelper.ToObject<List<VaccinationIndicator>>(filePath, ';')).OrderByDescending(x => x.Date).ToList();
        }

        public static async Task<List<Department>> GetDepartments(string filePath)
        {
            filePath = Path.Combine(filePath, Guid.NewGuid() + ".json");

            WebClient webClient = new();
            webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/7b4bc207-4e66-49d2-b1a5-26653e369b66", filePath);

            return await Json.ToObjectAsync<List<Department>>(File.ReadAllText(filePath));
        }
    }
}
