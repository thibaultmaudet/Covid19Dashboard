using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Covid19Dashboard.Helpers;

namespace Covid19Dashboard.Models
{
    public class DataTiles : ObservableCollection<DataTile>
    {
        public bool IsHomeTiles { get; set; }

        public Page Page { get; set; }

        public string CategoryTitle { get; set; }

        public void AddRange(IEnumerable<DataTile> dataTiles)
        {
            foreach (DataTile dataTile in dataTiles)
                Add(dataTile);
        }

        public IEnumerable<DataTile> GetHomeDataTiles()
        {
            return Items.Where(x => x.IsHomeTile);
        }
    }
}
