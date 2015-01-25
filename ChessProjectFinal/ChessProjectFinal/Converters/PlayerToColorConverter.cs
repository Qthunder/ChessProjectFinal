using System;
using System.Windows.Media;
using System.Windows.Data;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Converters
{
    class PlayerToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var player = (Player)value;
            return player == Player.White ? new SolidColorBrush(Colors.AntiqueWhite) : new SolidColorBrush(Colors.DarkSlateGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
