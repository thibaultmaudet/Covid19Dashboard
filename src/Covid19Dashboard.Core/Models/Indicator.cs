using Newtonsoft.Json;
using System;

namespace Covid19Dashboard.Core.Models
{
    public class Indicator
    {
        [JsonProperty("jour")]
        public DateTime Date { get; set; }
    }
}
