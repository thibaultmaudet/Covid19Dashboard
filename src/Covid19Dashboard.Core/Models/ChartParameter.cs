using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Covid19Dashboard.Core.Models
{
    public class ChartParameter
    {
        public List<ObservableCollection<ChartIndicator>> ChartIndicators { get; set; }

        public ChartType ChartType { get; set; }
    }
}
