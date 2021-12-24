using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; } = new HomeViewModel();

        public HomePage()
        {
            InitializeComponent();
        }

        private void TileControl_SeeMoreDetailsClick(object sender, EventArgs e)
        {
            ObservableCollection<ChartIndicator> chartIndicators = new ObservableCollection<ChartIndicator>();

            switch ((sender as HyperlinkButton).Tag as string)
            {
                case "IncidenceRate":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).Where(x => x.IncidenceRate.HasValue).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = Math.Round((decimal)epidemicIndicator.IncidenceRate, 2) });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Area, ChartIndicators = chartIndicators });
                    break;
                case "NewCase":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = epidemicIndicator.DailyConfirmedNewCases });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Bar, ChartIndicators = chartIndicators });
                    break;
                case "NewHospitalization":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = epidemicIndicator.NewHospitalization });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Bar, ChartIndicators = chartIndicators });
                    break;
                case "ReproductionRate":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).Where(x => x.ReproductionRate.HasValue).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = Math.Round((decimal)epidemicIndicator.ReproductionRate, 2) });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Area, ChartIndicators = chartIndicators });
                    break;
            }
        }
    }
}
