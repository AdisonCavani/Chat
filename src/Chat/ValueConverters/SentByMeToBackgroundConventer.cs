﻿using System;
using System.Globalization;
using System.Windows;

namespace Chat
{
    /// <summary>
    /// A converter that takes in a boolean if a message was sent by me, 
    /// and returns the correct background color
    /// </summary>
    public class SentByMeToBackgroundConventer : BaseValueConverter<SentByMeToBackgroundConventer>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Application.Current.FindResource("WordVeryLightBlueBrush") : Application.Current.FindResource("ForegroundLightBrush");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}