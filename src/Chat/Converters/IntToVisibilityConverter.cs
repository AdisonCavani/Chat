using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Chat.Converters;

public class IntToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is not int)
            throw new ArgumentException("Argument is not a type of int", nameof(value));

        if ((int)value != 0)
            return Visibility.Visible;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is not int)
            throw new ArgumentException("Argument is not a type of int", nameof(value));

        if ((int)value != 0)
            return Visibility.Visible;

        return Visibility.Collapsed;
    }
}
