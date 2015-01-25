using System.Collections.Generic;
using System.Windows.Data;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Converters
{
    public class PieceToDisplayConverter : IValueConverter
    {
        private static Dictionary<PieceType, string> Display = new Dictionary<PieceType, string>();

        static PieceToDisplayConverter()
        {
            Display.Add(PieceType.Pawn, "P");
            Display.Add(PieceType.Knight, "N");
            Display.Add(PieceType.Bishop, "B");
            Display.Add(PieceType.Queen, "QQ");
            Display.Add(PieceType.King, "K");
            Display.Add(PieceType.Rook, "R");
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var pieceType = (PieceType)value;
            return Display[pieceType];

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
