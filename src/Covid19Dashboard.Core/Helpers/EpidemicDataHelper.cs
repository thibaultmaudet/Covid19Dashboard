using Covid19Dashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Covid19Dashboard.Core.Helpers
{
    public class EpidemicDataHelper
    {
        public static string GetDailyConfirmedNewCasesValue(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
            {
                int? hospitalization = epidemicIndicators.FirstOrDefault(x => x.DailyConfirmedNewCases.HasValue)?.DailyConfirmedNewCases;

                return hospitalization.HasValue ? hospitalization.ToString() : "";
            }

            return "";
        }

        public static string GetDailyConfirmedNewCasesLastUpdate(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.FirstOrDefault(x => x.DailyConfirmedNewCases.HasValue)?.Date.ToShortDateString();

            return "";
        }

        public static string GetIncidenceRate(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.FirstOrDefault(x => x.IncidenceRate.HasValue).IncidenceRate.Value.ToString("0.00");

            return "";
        }

        public static string GetIncidenceRateLastUpdate(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.First(x => x.IncidenceRate.HasValue).Date.ToShortDateString();

            return "";
        }

        public static string GetNewHospitalization(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.FirstOrDefault(x => x.NewHospitalization.HasValue).NewHospitalization.Value.ToString();

            return "";
        }

        public static string GetNewHospitalizationLastUpdate(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.First(x => x.NewHospitalization.HasValue).Date.ToShortDateString();

            return "";
        }

        public static string GetReproductionRate(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.FirstOrDefault(x => x.ReproductionRate.HasValue).ReproductionRate.Value.ToString("0.00");

            return "";
        }

        public static string GetReproductionRateLastUpdate(List<EpidemicIndicator> epidemicIndicators)
        {
            if (epidemicIndicators != null && epidemicIndicators.Count > 0)
                return epidemicIndicators.First(x => x.ReproductionRate.HasValue).Date.ToShortDateString();

            return "";
        }
    }
}
