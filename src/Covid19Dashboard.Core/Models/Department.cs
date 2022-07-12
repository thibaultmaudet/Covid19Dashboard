using Newtonsoft.Json;

namespace Covid19Dashboard.Core.Models
{
    public class Department
    {
        [JsonProperty("dep_name")]
        public string Label { get; set; }

        [JsonProperty("num_dep")]
        public string Number { get; set; }

        [JsonProperty("region_name")]
        public string RegionLabel { get; set; }
    }
}
