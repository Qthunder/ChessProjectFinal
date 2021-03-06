﻿using System;
using System.Windows.Media;
using System.Windows.Data;

namespace ChessProjectFinal.Converters
{
    public class PointToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var point = (System.Windows.Point)value;
          
                var color = (point.X + point.Y)%2 == 0 ? Color.FromArgb(192, 89, 32, 8) : Colors.BurlyWood;
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
