using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;

namespace Covid19Dashboard.Models
{
    public class DataTile
    {
        public bool DisplayEvolution { get { return !(Evolution == default); } }

        public bool IsAverage { get; set; }

        public bool IsNationalIndicator { get; set; }

        public bool WithAverage { get; set; }

        public ChartType ChartType { get; set; }

        public double Evolution { get; set; }

        public int Digits { get; set; }

        public List<ObservableCollection<ChartIndicator>> ChartIndicators { get; set; }

        public string Data { get; set; }

        public string Description { get { return (Property + (IsAverage ? "Average" : "") + "Description").GetLocalized(); } }

        public string FullDescription { get { return Data + " " + Description; } }

        public string LastUpdate { get; set; }

        public string Property { get; set; }

        public string Title { get { return (Property + (IsAverage ? "Average" : "") + "Title").GetLocalized(); } }

        public Type IndicatorType { get; set; }
    }
}
