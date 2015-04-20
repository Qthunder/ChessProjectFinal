using System.Windows.Data;
using ChessProjectFinal.Entities;

namespace ChessProjectFinal.Converters
{
    public class PlayerTypeToBoolConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (PlayerType) value == PlayerType.AI;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool) value ? PlayerType.AI : PlayerType.HUMAN;
        }
    }
}
