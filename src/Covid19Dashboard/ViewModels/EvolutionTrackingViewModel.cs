using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Covid19Dashboard.ViewModels
{
    public class EvolutionTrackingViewModel : ObservableObject
    {
        public Data Data => Data.Instance;

        private DataTile selected;

        public DataTile Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public ObservableCollection<DataTile> EvolutionIndicators { get; private set; } = new ObservableCollection<DataTile>();

        public EvolutionTrackingViewModel()
        {
        }

        public async Task LoadDataAsync(ListDetailsViewState viewState)
        {
            EvolutionIndicators.Clear();

            foreach (var item in await GetIndicators())
                EvolutionIndicators.Add(item);

            if (viewState == ListDetailsViewState.Both)
                Selected = EvolutionIndicators.First();
        }

        public async Task<IEnumerable<DataTile>> GetIndicators()
        {
            Data.IsLoading = true;

            ObservableCollection<DataTile> tiles = new();

            List<Task> tasks = new();

            TaskScheduler uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("DailyConfirmedNewCases", false, true, 0, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("DailyConfirmedNewCases", ChartType.Bar, false, true, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("DailyConfirmedNewCases", ChartType.Line, true, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("PositiveCases", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("PositiveCases", ChartType.Bar, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("PositiveCases", ChartType.Line, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewHospitalization", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("NewHospitalization", ChartType.Bar, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("NewHospitalization", ChartType.Line, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("HospitalizedPatients", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("HospitalizedPatients", ChartType.Bar, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("HospitalizedPatients", ChartType.Line, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("NewIntensiveCarePatients", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("NewIntensiveCarePatients", ChartType.Bar, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("NewIntensiveCarePatients", ChartType.Line, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("IntensiveCarePatients", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("IntensiveCarePatients", ChartType.Bar, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("IntensiveCarePatients", ChartType.Line, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));
            tasks.Add(Task.Factory.StartNew(() =>
            {
                DataTile dataTile = TileHelper.SetDataTile("DeceasedPersons", false, false, true, 2, typeof(EpidemicIndicator));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("DeceasedPersons", ChartType.Bar, typeof(EpidemicIndicator)));
                dataTile.ChartIndicators.Add(TileHelper.GetChartEvolutionIndicators("DeceasedPersons", ChartType.Line, true, typeof(EpidemicIndicator)));

                tiles.Add(dataTile);
            }, CancellationToken.None, TaskCreationOptions.None, uiScheduler));

            await Task.WhenAll(tasks);

            Data.IsLoading = false;

            return tiles;
        }
    }
}
