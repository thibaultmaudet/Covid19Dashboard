using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Covid19Dashboard.Views
{
    public sealed partial class ChartPage : Page
    {
        public ChartViewModel ViewModel { get; } = new ChartViewModel();

        public ChartPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ChartParameter)
            {
                ViewModel.ChartType = (e.Parameter as ChartParameter).ChartType;
                ViewModel.Source = (e.Parameter as ChartParameter).ChartIndicators;
            }
        }
    }
}
