using System.Collections.Generic;
using System.Windows.Data;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Converters
{
    public class PieceToDisplayConverter : IValueConverter
    {
        private static readonly Dictionary<PieceType, string> PIECE_TYPE = new Dictionary<PieceType, string>();
        private static readonly Dictionary<Player, string> PIECE_COLOR = new Dictionary<Player, string>();


        static PieceToDisplayConverter()
        {   PIECE_COLOR.Add(Player.White,"white");
            PIECE_COLOR.Add(Player.Black,"black");
            PIECE_TYPE.Add(PieceType.Pawn, "pawn");
            PIECE_TYPE.Add(PieceType.Knight, "knight");
            PIECE_TYPE.Add(PieceType.Bishop, "bishop");
            PIECE_TYPE.Add(PieceType.Queen, "queen");
            PIECE_TYPE.Add(PieceType.King, "king");
            PIECE_TYPE.Add(PieceType.Rook, "rook");
        }

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var piece = (PieceStruct)value;
            if (piece == null)
                return null;
            var source = "http://www.wpclipart.com/recreation/games/chess/chess_set_1/chess_piece_" +PIECE_COLOR[piece.Color] + "_" + PIECE_TYPE[piece.Piece] + "_T.png";
            return source;

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
