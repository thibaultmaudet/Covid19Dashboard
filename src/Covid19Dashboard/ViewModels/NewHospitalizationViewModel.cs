using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Covid19Dashboard.Core.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Covid19Dashboard.ViewModels
{
    public class NewHospitalizationViewModel : ObservableObject
    {
        public ObservableCollection<EpidemicIndicator> Source { get; set; }

        public NewHospitalizationViewModel()
        {
            Source = new ObservableCollection<EpidemicIndicator>();
        }

        public void LoadData(List<EpidemicIndicator> epidemicIndicators)
        {
            IEnumerable<EpidemicIndicator> indicators = epidemicIndicators.Where(x => x.Date != null && x.NewHospitalization.HasValue);

            Source = new ObservableCollection<EpidemicIndicator>(indicators.Skip(indicators.Count() - 70));
        }
    }
}
