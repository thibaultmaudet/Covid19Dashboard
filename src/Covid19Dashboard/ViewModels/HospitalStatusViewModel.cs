using System.Collections.Generic;
using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Core.Services;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.Storage;
using Windows.UI.Core;

namespace Covid19Dashboard.ViewModels
{
    public class HospitalStatusViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private List<DataTile> dataTiles;

        public List<DataTile> DataTiles
        {
            get { return dataTiles; }
            set { SetProperty(ref dataTiles, value); }
        }

        public HospitalStatusViewModel()
        {
            if (Data.EpidemicIndicators == null)
            {
                _ = Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    Data.EpidemicIndicators = await EpidemicDataService.GetEpedimicIndicatorsAsync(ApplicationData.Current.TemporaryFolder.Path, Area.National);

                    UpdateDataTiles();
                });
            }
            else
                UpdateDataTiles();

        }

        public void UpdateDataTiles()
        {
            EpidemicDataHelper.IndicatorType = typeof(EpidemicIndicator);

            DataTiles = new List<DataTile>
            {
                TileHelper.SetDataTile("NewHospitalization", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("HospitalizedPatients", ChartType.Area, false, false, false, 0),
                TileHelper.SetDataTile("NewHospitalization", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("NewIntensiveCarePatients", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("IntensiveCarePatients", ChartType.Area, false, false, false, 0),
                TileHelper.SetDataTile("NewIntensiveCarePatients", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("NewDeceasedPersons", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("DeceasedPersons", ChartType.Area, false, true, false, 0),
                TileHelper.SetDataTile("NewDeceasedPersons", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("NewReturnHome", ChartType.Bar, false, false, false, 0),
                TileHelper.SetDataTile("NewReturnHome", ChartType.Area, true, false, true, 0),
                TileHelper.SetDataTile("OccupationRate", ChartType.Area, false, false, true, 2)
            };
        }
    }
}
