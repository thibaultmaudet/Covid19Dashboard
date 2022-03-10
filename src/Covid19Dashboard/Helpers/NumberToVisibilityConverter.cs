using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Covid19Dashboard.Helpers
{
    public class NumberToVisibilityConverter : IValueConverter
    {
        public int MaxNumberHidden { get; set; } = int.MaxValue;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool converted = double.TryParse(value.ToString(), out double number);

            return converted ? number != int.MaxValue && MaxNumberHidden == int.MaxValue || (MaxNumberHidden != int.MaxValue && number <= MaxNumberHidden) ? Visibility.Visible : Visibility.Collapsed : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new ArgumentException("value must be an integer!");
        }
    }
}
