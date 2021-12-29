using Newtonsoft.Json;
using System;

namespace Covid19Dashboard.Core.Models
{
    public class EpidemicIndicator
    {
        private float? incidenceRate;
        private float? positivityRate;
        private float? reproductionRate;

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("conf_j1")]
        public int? DailyConfirmedNewCases { get; set; }

        [JsonProperty("dc_tot")]
        public int? DeceasedPersons { get; set; }

        [JsonProperty("hosp")]
        public int? HospitalizedPatients { get; set; }

        [JsonProperty("rea")]
        public int? IntensiveCarePatients { get; set; }

        [JsonProperty("incid_hosp")]
        public int? NewHospitalization { get; set; } 

        [JsonProperty("pos")]
        public int? PositiveCases { get; set; }

        [JsonProperty("tx_incid")]
        public float? IncidenceRate
        {
            get { return incidenceRate; }
            set 
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    float.TryParse(value.ToString().Replace('.', ','), out float result);
                    incidenceRate = result;
                }
                else
                    incidenceRate = null;
            }
        }

        [JsonProperty("tx_pos")]
        public float? PositivityRate
        {
            get { return positivityRate; }
            set
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    float.TryParse(value.ToString().Replace('.', ','), out float result);
                    positivityRate = result;
                }
                else
                    positivityRate = null;
            }
        }

        [JsonProperty("R")]
        public float? ReproductionRate
        {
            get { return reproductionRate; }
            set
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    float.TryParse(value.ToString().Replace('.', ','), out float result);
                    reproductionRate = result;
                }
                else
                    reproductionRate = null;
            }
        }
    }
}
