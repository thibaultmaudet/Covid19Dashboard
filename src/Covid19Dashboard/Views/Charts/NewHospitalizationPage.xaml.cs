using System.Collections.Generic;
using System.Linq;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Covid19Dashboard.Views.Charts
{
    public sealed partial class NewHospitalizationPage : Page
    {
        public NewHospitalizationViewModel ViewModel { get; } = new NewHospitalizationViewModel();

        public NewHospitalizationPage()
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
