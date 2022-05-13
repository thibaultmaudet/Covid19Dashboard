using Newtonsoft.Json;

namespace Covid19Dashboard.Core.Models
{
    public class VaccinationIndicator : Indicator
    {
        [JsonProperty("clage_vacsi")]

        public new int? AgeClass
        {
            get { return base.AgeClass; }
            set { base.AgeClass = value; }
        }

        [JsonProperty("n_complet_e")]
        public int? NewCompleteVaccinations { get; set; }

        [JsonProperty("n_rappel_e")]
        public int? NewFirstBoosterDoses { get; set; }

        [JsonProperty("n_dose1_e")]
        public int? NewFirstDoses { get; set; }

        [JsonProperty("n_2_rappel_e")]
        public int? NewSecondBoosterDoses { get; set; }

        [JsonProperty("n_cum_complet_e")]
        public int? TotalCompleteVaccinations { get; set; }

        [JsonProperty("n_cum_rappel_e")]
        public int? TotalFirstBoosterDoses { get; set; }

        [JsonProperty("n_cum_2_rappel_e")]
        public int? TotalSecondBoosterDoses { get; set; }

        [JsonProperty("n_cum_dose1_e")]
        public int? TotalFirstDoses { get; set; }

        [JsonProperty("couv_complet_e")]
        public float? CompleteVaccinationsCoverage { get; set; }

        [JsonProperty("couv_rappel_e")]
        public float? FirstBoosterDosesCoverage { get; set; }

        [JsonProperty("couv_dose1_e")]
        public float? FirstDosesCoverage { get; set; }


        [JsonProperty("couv_2_rappel_e")]
        public float? SecondBoosterDosesCoverage { get; set; }
    }
}
