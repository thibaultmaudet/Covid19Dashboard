using System;

using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Covid19Dashboard.Helpers.Converters
{
    public class NumberToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            float.TryParse(value.ToString(), out float number);

            if ((parameter as string) == "background")
                return new SolidColorBrush(number > 0 ? Color.FromArgb(255, 255, 244, 243) : Color.FromArgb(100, 217, 255, 235));

            if ((parameter as string) == "foreground")
                return new SolidColorBrush(number > 0 ? Colors.Red : Colors.DarkGreen);

            throw new ArgumentException("parameter has no know value.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
