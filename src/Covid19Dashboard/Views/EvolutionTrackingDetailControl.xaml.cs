using Covid19Dashboard.Models;
using Covid19Dashboard.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Views
{
    public sealed partial class EvolutionTrackingDetailControl : UserControl
    {
        public EvolutionTrackingViewModel ViewModel { get; } = new EvolutionTrackingViewModel();

        public DataTile SelectedIndicator
        {
            get { return GetValue(SelectedIndicatorProperty) as DataTile; }
            set { SetValue(SelectedIndicatorProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndicatorProperty = DependencyProperty.Register("SelectedIndicator", typeof(DataTile), typeof(EvolutionTrackingDetailControl), new PropertyMetadata(null, OnSelectedIndicatorPropertyChanged));

        public EvolutionTrackingDetailControl()
        {
            InitializeComponent();
        }

        private static void OnSelectedIndicatorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EvolutionTrackingDetailControl control = d as EvolutionTrackingDetailControl;
            control.ViewModel.Selected = control.SelectedIndicator;
        }
    }
}
