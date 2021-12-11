using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Covid19Dashboard.Core.Helpers;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Helpers;
using Covid19Dashboard.ViewModels;
using Covid19Dashboard.Views.Charts;
using Newtonsoft.Json;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class HomePage : Page
    {
        public HomeViewModel ViewModel { get; } = new HomeViewModel();

        public HomePage()
        {
            InitializeComponent();
        }

        private void NewCasesHyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewCasesPage), ViewModel.EpidemicIndicators);
        }

        private void IncidenceRateHyperlinkButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.Navigate(typeof(IncidenceRatePage), ViewModel.EpidemicIndicators);
        }
    }
}
