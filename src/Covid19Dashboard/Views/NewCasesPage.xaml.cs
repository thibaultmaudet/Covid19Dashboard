using System.Collections.Generic;
using System.Linq;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Covid19Dashboard.Views
{
    public sealed partial class NewCasesPage : Page
    {
        public NewCasesViewModel ViewModel { get; } = new NewCasesViewModel();

        public NewCasesPage()
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
