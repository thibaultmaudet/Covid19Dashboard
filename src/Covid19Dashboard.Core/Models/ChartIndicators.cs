using System.Collections.ObjectModel;

namespace Covid19Dashboard.Core.Models
{
    public class ChartIndicators : ObservableCollection<ChartIndicator>
    {
        public ChartType ChartType { get; set; }

        public string Name { get; set; }

        public void Add(ObservableCollection<ChartIndicator> chartIndicators)
        {
            foreach (ChartIndicator chartIndicator in chartIndicators)
                Add(chartIndicator);
        }
    }
}
