using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Covid19Dashboard.Core.Models
{
    public class ChartParameter
    {
        public ObservableCollection<ChartIndicator> ChartIndicators { get; set; }

        public ChartType ChartType { get; set; }
    }
}
