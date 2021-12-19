using System.Collections.Generic;
using System.Linq;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Covid19Dashboard.Views.Charts
{
    public sealed partial class ReproductionRatePage : Page
    {
        public ReproductionRateViewModel ViewModel { get; } = new ReproductionRateViewModel();

        public ReproductionRatePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is List<EpidemicIndicator>)
                ViewModel.LoadData((e.Parameter as List<EpidemicIndicator>).OrderBy(x => x.Date).ToList());
        }
    }
}
