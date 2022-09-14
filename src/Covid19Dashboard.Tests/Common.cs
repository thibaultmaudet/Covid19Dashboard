using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;

namespace Covid19Dashboard.Tests
{
    public class Common
    {
        public static List<EpidemicIndicator> EpidemicIndicators { get; set; }

        public static List<VaccinationIndicator> VaccinationIndicators { get; set; }

        public static async Task<List<EpidemicIndicator>> GetEpidemicIndicatorsAsync()
        {
            return (await CsvHelper.ToObject<List<EpidemicIndicator>>("Assets\\eaca70f4-ed27-42c8-b878-ab34c930d3df.csv", ',')).OrderByDescending(x => x.Date).ToList();
        }

        public static async Task<List<VaccinationIndicator>> GetVaccinationIndicatorsAsync()
        {
            return (await CsvHelper.ToObject<List<VaccinationIndicator>>("Assets\\4ecd4b41-333f-4bb2-ad0b-f757ba61b440.csv", ';')).OrderByDescending(x => x.Date).ToList();
        }
    }
}
