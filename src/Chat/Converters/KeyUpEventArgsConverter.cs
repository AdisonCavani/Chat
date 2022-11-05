using System;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;

namespace Chat.Converters;

public class KeyUpEventArgsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var args = (KeyRoutedEventArgs)value;
        return args;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var args = (KeyRoutedEventArgs)value;
        return args;
    }
}
