﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Covid19Dashboard.Core.Models
{
    public class EpidemicIndicator : INotifyPropertyChanged
    {
        private DateTime date;

        private int? dailyConfirmedNewCases;
        private int? newHospitalization;

        private float? incidenceRate;
        private float? reproductionRate;

        [JsonProperty("date")]
        public DateTime Date
        {
            get { return date; }
            set { SetProperty(ref date, value); }
        }

        [JsonProperty("conf_j1")]
        public int? DailyConfirmedNewCases
        {
            get { return dailyConfirmedNewCases; }
            set { SetProperty(ref dailyConfirmedNewCases, value); }
        }

        [JsonProperty("incid_hosp")]
        public int? NewHospitalization
        {
            get { return newHospitalization; }
            set { SetProperty(ref newHospitalization, value); }
        }

        [JsonProperty("tx_incid")]
        public float? IncidenceRate
        {
            get { return incidenceRate; }
            set 
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    float.TryParse(value.ToString().Replace('.', ','), out float result);
                    SetProperty(ref incidenceRate, result);
                }
                else
                    SetProperty(ref incidenceRate, null);
            }
        }

        [JsonProperty("R")]
        public float? ReproductionRate
        {
            get { return reproductionRate; }
            set
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    float.TryParse(value.ToString().Replace('.', ','), out float result);
                    SetProperty(ref reproductionRate, result);
                }
                else
                    SetProperty(ref reproductionRate, null);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return;

            storage = value;
            OnPropertyChanged(propertyName);
        }
    }
}
