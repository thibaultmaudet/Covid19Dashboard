﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Covid19Dashboard.Core.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Covid19Dashboard.ViewModels
{
    public class NewHospitalizationViewModel : ObservableObject
    {
        public ObservableCollection<ChartIndicator> Source { get; set; }

        public NewHospitalizationViewModel()
        {
            Source = new ObservableCollection<ChartIndicator>();
        }

        public void LoadData(List<EpidemicIndicator> epidemicIndicators)
        {
            IEnumerable<EpidemicIndicator> indicators = epidemicIndicators.Where(x => x.Date != null && x.NewHospitalization.HasValue);

            indicators = indicators.Skip(indicators.Count() - 70);

            foreach (EpidemicIndicator epidemicIndicator in indicators)
                Source.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = epidemicIndicator.NewHospitalization });
        }
    }
}