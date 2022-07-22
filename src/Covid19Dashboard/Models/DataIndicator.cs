using System;

using Covid19Dashboard.Core.Enums;

namespace Covid19Dashboard.Models
{
    public class DataIndicator
    {
        public bool IsAverage { get; set; }

        public bool IsEvolutionIndicator { get; set; }

        public bool IsNationalIndicator { get; set; }

        public bool WithAverage { get; set; }

        public ChartType ChartType { get; set; }

        public int Digits { get; set; }

        public string Property { get; set; }

        public Type IndicatorType { get; set; }
    }
}
