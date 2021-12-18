using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    public sealed partial class ChartPageControl : UserControl
    {
        public ChartType ChartType { get; set; }

        public ObservableCollection<ChartIndicator> ChartIndicators { get; set; }

        public ChartPageControl()
        {
            InitializeComponent();
        }
    }
}
