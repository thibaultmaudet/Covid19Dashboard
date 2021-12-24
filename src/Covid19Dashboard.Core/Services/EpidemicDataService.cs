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
        public static async Task<List<EpidemicIndicator>> GetEpedimicIndicators(string filePath)
        {
            filePath = Path.Combine(filePath, Guid.NewGuid() + ".csv");
            
            // Download the summary of indicators for monitoring the COVID-19 epidemic csv file (national).
            WebClient webClient = new WebClient();
            webClient.DownloadFile("https://www.data.gouv.fr/fr/datasets/r/f335f9ea-86e3-4ffa-9684-93c009d5e617", filePath);

            return (await CsvHelper.ToObject<List<EpidemicIndicator>>(filePath)).OrderByDescending(x => x.Date).ToList();
        }
    }
}
