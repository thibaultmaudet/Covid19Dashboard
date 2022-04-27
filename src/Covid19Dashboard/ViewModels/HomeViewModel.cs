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
    public class HomeViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private DataTiles epidemiologyDataTiles;
        private DataTiles hospitalDataTiles;
        private DataTiles vaccinationDataTiles;

        public DataTiles EpidemiologyDataTiles
        {
            get { return epidemiologyDataTiles; }
            set { SetProperty(ref epidemiologyDataTiles, value); }
        }

        public DataTiles HospitalDataTiles
        {
            get { return hospitalDataTiles; }
            set { SetProperty(ref hospitalDataTiles, value); }
        }

        public DataTiles VaccinationDataTiles
        {
            get { return vaccinationDataTiles; }
            set { SetProperty(ref vaccinationDataTiles, value); }
        }

        public HomeViewModel()
        {

        }

        public async Task UpdateDataTilesAsync(bool resetIndicator)
        {
            Data.IsLoading = true;

            await EpidemicDataHelper.DownloadEpidemicIndicatorsFiles(ApplicationData.Current.TemporaryFolder.Path);

            if (App.DataTiles == null || resetIndicator)
                App.DataTiles = TileHelper.GetDataTiles();

            EpidemiologyDataTiles = App.DataTiles.First(x => x.Page == Page.Epidemiologic);
            HospitalDataTiles = App.DataTiles.First(x => x.Page == Page.Hospital);
            VaccinationDataTiles = App.DataTiles.First(x => x.Page == Page.Vaccination);

            Data.IsLoading = false;
        }
    }
}
