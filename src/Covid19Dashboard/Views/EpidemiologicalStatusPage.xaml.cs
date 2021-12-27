using System;
using System.Collections.ObjectModel;
using System.Linq;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class EpidemiologicalStatusPage : Page
    {
        public EpidemiologicalStatusViewModel ViewModel { get; } = new EpidemiologicalStatusViewModel();

        public EpidemiologicalStatusPage()
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
                case "PositiveCases":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).Where(x => x.PositiveCases.HasValue).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = epidemicIndicator.PositiveCases });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Bar, ChartIndicators = chartIndicators });
                    break;
                case "PositiveCasesWeeklyAverage":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).Where(x => x.PositiveCases.HasValue).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = EpidemicDataHelper.GetPositiveConfirmedNewCasesWeeklyAverage(App.EpidemicIndicators, epidemicIndicator.Date) });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Area, ChartIndicators = chartIndicators });
                    break;
                case "PositivityRate":
                    foreach (EpidemicIndicator epidemicIndicator in App.EpidemicIndicators.OrderBy(x => x.Date).Where(x => x.PositivityRate.HasValue).TakeLast(70))
                        chartIndicators.Add(new ChartIndicator() { Date = epidemicIndicator.Date, Value = Math.Round((decimal)epidemicIndicator.PositivityRate, 2) });

                    Frame.Navigate(typeof(ChartPage), new ChartParameter() { ChartType = ChartType.Area, ChartIndicators = chartIndicators });
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
