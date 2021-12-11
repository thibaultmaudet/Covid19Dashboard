using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Covid19Dashboard.Core.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Covid19Dashboard.ViewModels
{
    public class IncidenceRateViewModel : ObservableObject
    {
        public ObservableCollection<EpidemicIndicator> Source { get; set; }

        public IncidenceRateViewModel()
        {
            Source = new ObservableCollection<EpidemicIndicator>();
        }

        public void LoadData(List<EpidemicIndicator> epidemicIndicators)
        {
            IEnumerable<EpidemicIndicator> indicators = epidemicIndicators.Where(x => x.Date != null && x.IncidenceRate.HasValue);

            Source = new ObservableCollection<EpidemicIndicator>(indicators.Skip(indicators.Count() - 70));
        }
    }
}
