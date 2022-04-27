using System.Linq;
using System.Threading.Tasks;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

using Windows.Storage;

namespace Covid19Dashboard.ViewModels
{
    public class HospitalStatusViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private DataTiles dataTiles;

        public DataTiles DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public HospitalStatusViewModel()
        {

        }

        public async Task UpdateDataTilesAsync(bool resetIndicator)
        {
            Data.IsLoading = true;

            await EpidemicDataHelper.DownloadEpidemicIndicatorsFiles(ApplicationData.Current.TemporaryFolder.Path);

            if (App.DataTiles == null || resetIndicator)
                App.DataTiles = TileHelper.GetDataTiles();

            DataTiles = App.DataTiles.First(x => x.Page == Page.Hospital);

            Data.IsLoading = false;
        }
    }
}
