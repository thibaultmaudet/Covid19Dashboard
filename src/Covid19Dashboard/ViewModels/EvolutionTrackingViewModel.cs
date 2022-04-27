using System.Collections.ObjectModel;
using System.Linq;

using Covid19Dashboard.Core;
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

        private DataTiles DataTiles { get { return App.DataTiles.First(x => x.Page == Page.Evolution); } }

        public ObservableCollection<DataTile> EvolutionIndicators { get; private set; } = new ObservableCollection<DataTile>();

        public EvolutionTrackingViewModel()
        {
        }

        public void LoadData(ListDetailsViewState viewState)
        {
            EvolutionIndicators.Clear();

            foreach (DataTile item in DataTiles)
                EvolutionIndicators.Add(item);

            if (viewState == ListDetailsViewState.Both)
                Selected = EvolutionIndicators.First();
        }
    }
}
