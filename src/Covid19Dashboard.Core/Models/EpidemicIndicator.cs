using Newtonsoft.Json;
using System;

namespace Covid19Dashboard.Core.Models
{
    public class EpidemicIndicator : Indicator
    {
        private float? incidenceRate;
        private float? occupationRate;
        private float? positivityRate;
        private float? reproductionRate;
        
        [JsonProperty("date")]
        public new DateTime Date 
        {
            get { return base.Date; }
            set { base.Date = value; }
        }

        [JsonProperty("conf_j1")]
        public int? DailyConfirmedNewCases { get; set; }

        [JsonProperty("dc_tot")]
        public int? DeceasedPersons { get; set; }

        [JsonProperty("hosp")]
        public int? HospitalizedPatients { get; set; }

        [JsonProperty("rea")]
        public int? IntensiveCarePatients { get; set; }

        [JsonProperty("incid_dchosp")]
        public int? NewDeceasedPersons { get; set; }

        [JsonProperty("incid_hosp")]
        public int? NewHospitalization { get; set; } 

        [JsonProperty("incid_rea")]
        public int? NewIntensiveCarePatients { get; set; } 

        [JsonProperty("incid_rad")]
        public int? NewReturnHome{ get; set; } 

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

        [JsonProperty("TO")]
        public float? OccupationRate
        {
            get { return occupationRate; }
            set
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    float.TryParse(value.ToString().Replace('.', ','), out float result);
                    occupationRate = result * 100;
                }
                else
                    occupationRate = null;
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
