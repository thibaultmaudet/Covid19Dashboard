using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Covid19Dashboard.Core.Models;

namespace Covid19Dashboard.Core
{
    public class Data : INotifyPropertyChanged
    {
        private static volatile Data instance;
        private static readonly object syncRoot = new();

        private List<Department> departments;

        private bool isLoading;

        private string selectedDepartment;

        public static Data Instance
        {
            get
            {
                if (instance == null)
                    lock (syncRoot)
                        instance = new Data();

                return instance;
            }
        }

        public bool IsDepartementIndicatorsDownloaded { get; set; }

        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public List<EpidemicIndicator> EpidemicIndicators { get; set; }

        public List<VaccinationIndicator> VaccinationIndicators { get; set; }

        public List<Department> Departments
        {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }

        public string SelectedDepartment
        {
            get { return selectedDepartment; }
            set { SetProperty(ref selectedDepartment, value); }
        }

        private Data() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return;

            storage = value;
            OnPropertyChanged(propertyName);
        }
    }
}
