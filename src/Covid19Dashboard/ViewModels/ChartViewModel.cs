using System.Collections.ObjectModel;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Covid19Dashboard.ViewModels
{
    public class ChartViewModel : ObservableObject
    {
        private ChartType chartType;

        private ObservableCollection<ChartIndicator> source;

        public ChartType ChartType
        {
            get { return chartType; }
            set { SetProperty(ref chartType, value); }
        }

        public ObservableCollection<ChartIndicator> Source
        {
            get { return source; }
            set { SetProperty(ref source, value); }
        }

        public ChartViewModel()
        {

        }
    }
}
