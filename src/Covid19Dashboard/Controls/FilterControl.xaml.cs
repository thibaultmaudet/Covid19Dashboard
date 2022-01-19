using Covid19Dashboard.Core;
using System;
using Windows.UI.Xaml.Controls;

namespace Covid19Dashboard.Controls
{
    public sealed partial class FilterControl : UserControl
    {
        public Data Data => Data.Instance;

        public event EventHandler FilterChanged;

        public FilterControl()
        {
            InitializeComponent();
        }

        private void FiltersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterChanged?.Invoke(sender, new EventArgs());
        }

        private void ResetFiltersButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Data.SelectedDepartment = "";

            FilterChanged?.Invoke(sender, new EventArgs());
        }
    }
}
