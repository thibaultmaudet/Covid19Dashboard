using System;

using Newtonsoft.Json;

namespace Covid19Dashboard.Core.Models
{
    public class Indicator
    {
        [JsonProperty("jour")]
        public DateTime Date { get; set; }

        public int? AgeClass { get; set; }

        [JsonProperty("dep")]
        public string Department { get; set; }

        [JsonProperty("lib_dep")]
        public string DepartmentLabel { get; set; }
    }
}
