using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Covid19Dashboard.Helpers
{
    public class NumberToVisibilityConverter : IValueConverter
    {
        public int MaxNumberHidden { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool.TryParse(parameter.ToString(), out bool inverted);

            return !inverted && (int)value > MaxNumberHidden ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new ArgumentException("value must be an integer!");
        }
    }
}
