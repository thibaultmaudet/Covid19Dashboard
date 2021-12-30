using Newtonsoft.Json;
using System;

namespace Covid19Dashboard.Core.Models
{
    public class VaccinationIndicator : Indicator
    {
        [JsonProperty("n_complet")]
        public int? NewCompleteVaccinations { get; set; }

        [JsonProperty("n_rappel")]
        public int? NewFirstBoosterDoses { get; set; }

        [JsonProperty("n_dose1")]
        public int? NewFirstDoses { get; set; }

        [JsonProperty("n_cum_complet")]
        public int? TotalCompleteVaccinations { get; set; }

        [JsonProperty("n_cum_rappel")]
        public int? TotalFirstBoosterDoses { get; set; }

        [JsonProperty("n_cum_dose1")]
        public int? TotalFirstDoses { get; set; }

        [JsonProperty("couv_complet")]
        public float? CompleteVaccinationsCoverage { get; set; }

        [JsonProperty("couv_rappel")]
        public float? FirstBoosterDosesCoverage { get; set; }

        [JsonProperty("couv_dose1")]
        public float? FirstDosesCoverage { get; set; }
    }
}
