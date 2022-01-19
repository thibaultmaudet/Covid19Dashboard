using Covid19Dashboard.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Covid19Dashboard.Core
{
    public class Data : INotifyPropertyChanged
    {
        private static volatile Data instance;
        private static readonly object syncRoot = new object();

        private ObservableCollection<KeyValuePair<string, string>> departments;

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

        public List<EpidemicIndicator> EpidemicIndicators { get; set; }

        public List<VaccinationIndicator> VaccinationIndicators { get; set; }

        public ObservableCollection<KeyValuePair<string, string>> Departments
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
