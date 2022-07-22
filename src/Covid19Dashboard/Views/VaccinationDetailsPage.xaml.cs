using System;
using System.Collections.ObjectModel;
using System.Linq;

using Covid19Dashboard.Core.Models;
using Covid19Dashboard.ViewModels;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Covid19Dashboard.Views
{
    public sealed partial class VaccinationDetailsPage : Page
    {
        public VaccinationDetailsViewModel ViewModel { get; } = new VaccinationDetailsViewModel();

        public VaccinationDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.Date = Convert.ToDateTime(e.Parameter as string);

            ViewModel.LoadData();
        }

        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            switch (e.Column.Tag.ToString())
            {
                case "NewFirstDoses":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.NewFirstDoses ascending select item) : from item in ViewModel.Source orderby item.NewFirstDoses descending select item);
                    break;
                case "NewCompleteVaccinations":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.NewCompleteVaccinations ascending select item) : from item in ViewModel.Source orderby item.NewCompleteVaccinations descending select item);
                    break;
                case "NewFirstBoosterDoses":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.NewFirstBoosterDoses ascending select item) : from item in ViewModel.Source orderby item.NewFirstBoosterDoses descending select item);
                    break;
                case "NewSecondBoosterDoses":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.NewSecondBoosterDoses ascending select item) : from item in ViewModel.Source orderby item.NewSecondBoosterDoses descending select item);
                    break;
                case "TotalFirstDoses":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.TotalFirstDoses ascending select item) : from item in ViewModel.Source orderby item.TotalFirstDoses descending select item);
                    break;
                case "TotalCompleteVaccinations":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.TotalCompleteVaccinations ascending select item) : from item in ViewModel.Source orderby item.TotalCompleteVaccinations descending select item);
                    break;
                case "TotalFirstBoosterDoses":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.TotalFirstBoosterDoses ascending select item) : from item in ViewModel.Source orderby item.TotalFirstBoosterDoses descending select item);
                    break;
                case "TotalSecondBoosterDoses":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.TotalSecondBoosterDoses ascending select item) : from item in ViewModel.Source orderby item.TotalSecondBoosterDoses descending select item);
                    break;
                case "FirstDosesCoverage":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.FirstDosesCoverage ascending select item) : from item in ViewModel.Source orderby item.FirstDosesCoverage descending select item);
                    break;
                case "CompleteVaccinationsCoverage":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.CompleteVaccinationsCoverage ascending select item) : from item in ViewModel.Source orderby item.CompleteVaccinationsCoverage descending select item);
                    break;
                case "FirstBoosterDosesCoverage":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.FirstBoosterDosesCoverage ascending select item) : from item in ViewModel.Source orderby item.FirstBoosterDosesCoverage descending select item);
                    break;
                case "SecondBoosterDosesCoverage":
                    VaccinationDetailsDataGrid.ItemsSource = new ObservableCollection<VaccinationIndicator>(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.SecondBoosterDosesCoverage ascending select item) : from item in ViewModel.Source orderby item.SecondBoosterDosesCoverage descending select item);
                    break;
                case "AgeLabel":
                default:
                    VaccinationIndicator allAgeVaccinationIndicator = new();

                    allAgeVaccinationIndicator = ViewModel.Source.First(x => x.AgeClass == 0);

                    if (allAgeVaccinationIndicator != null)
                        ViewModel.Source.Remove(allAgeVaccinationIndicator);

                    ObservableCollection<VaccinationIndicator> vaccinationIndicators = new(e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending ? (from item in ViewModel.Source orderby item.AgeClass ascending select item) : from item in ViewModel.Source orderby item.AgeClass descending select item);


                    if (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending)
                        vaccinationIndicators.Add(allAgeVaccinationIndicator);
                    else
                        vaccinationIndicators.Insert(0, allAgeVaccinationIndicator);

                    ViewModel.Source = vaccinationIndicators;
                    VaccinationDetailsDataGrid.ItemsSource = ViewModel.Source;
                    break;
            }

            e.Column.SortDirection = (e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Descending) ? DataGridSortDirection.Ascending : DataGridSortDirection.Descending;

            foreach (var column in VaccinationDetailsDataGrid.Columns)
                if (column.Tag.ToString() != e.Column.Tag.ToString())
                    column.SortDirection = null;
        }

        private void FilterControl_FilterChanged(object sender, EventArgs e)
        {
            ViewModel.LoadData();
        }
    }
}
