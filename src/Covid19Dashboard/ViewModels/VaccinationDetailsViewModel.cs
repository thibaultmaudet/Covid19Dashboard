using System;
using System.Collections.ObjectModel;
using System.Linq;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;

using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace Covid19Dashboard.ViewModels
{
    public class VaccinationDetailsViewModel : ObservableObject
    {
        private static Data Data => Data.Instance;

        private ObservableCollection<VaccinationIndicator> source;

        public DateTime Date { get; set; }

        public ObservableCollection<VaccinationIndicator> Source
        {
            get { return source; }
            set { SetProperty(ref source, value); }
        }

        public VaccinationDetailsViewModel()
        {
            Source = new();
        }

        public void LoadData()
        {
            Source.Clear();

            VaccinationIndicator allAgeVaccinationIndicator = new();

            foreach (VaccinationIndicator vaccinationIndicator in EpidemicDataHelper.GetValuesByAge(Data.VaccinationIndicators, Date))
                if (vaccinationIndicator.AgeClass == 0)
                    allAgeVaccinationIndicator = vaccinationIndicator;
                else
                    Source.Add(vaccinationIndicator);

            Source = new(from item in Source
                         orderby item.AgeClass ascending
                         select item);

            Source.Add(allAgeVaccinationIndicator);
        }
    }
}
