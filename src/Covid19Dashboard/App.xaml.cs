using System;
using System.Collections.ObjectModel;

using Covid19Dashboard.Core;
using Covid19Dashboard.Core.Models;
using Covid19Dashboard.Models;
using Covid19Dashboard.Services;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Covid19Dashboard
{
    public sealed partial class App : Application
    {
        private Data Data => Data.Instance;

        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public static ObservableCollection<DataTiles> DataTiles { get; set; }

        public App()
        {
            InitializeComponent();
            UnhandledException += OnAppUnhandledException;

            // Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (!args.PrelaunchActivated)
            {
                await ApplicationData.Current.TemporaryFolder.DeleteAsync(StorageDeleteOption.PermanentDelete);

                await ActivationService.ActivateAsync(args);
            }

            LiveCharts.Configure(config => config.AddSkiaSharp().AddDefaultMappers().AddLightTheme().HasMap<ChartIndicator>((chartIndicator, point) =>
            {
                double.TryParse(chartIndicator.Value.ToString(), out double result);
                point.PrimaryValue = result;
                point.SecondaryValue = chartIndicator.Date.Ticks;
            }));
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private void OnAppUnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            // TODO WTS: Please log and handle the exception as appropriate to your scenario
            // For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.HomePage), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }
    }
}
