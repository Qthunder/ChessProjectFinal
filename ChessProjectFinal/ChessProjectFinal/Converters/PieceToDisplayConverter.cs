using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Data;
using ChessProjectFinal.Entities;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Converters
{
    public class PieceToDisplayConverter : IValueConverter
    {
        private static readonly Dictionary<PieceType, string> PIECE_TYPE = new Dictionary<PieceType, string>();
        private static readonly Dictionary<Player, string> PIECE_COLOR = new Dictionary<Player, string>();


        static PieceToDisplayConverter()
        {   PIECE_COLOR.Add(Player.WHITE,"white");
            PIECE_COLOR.Add(Player.BLACK,"black");
            PIECE_TYPE.Add(PieceType.PAWN, "pawn");
            PIECE_TYPE.Add(PieceType.KNIGHT, "knight");
            PIECE_TYPE.Add(PieceType.BISHOP, "bishop");
            PIECE_TYPE.Add(PieceType.QUEEN, "queen");
            PIECE_TYPE.Add(PieceType.KING, "king");
            PIECE_TYPE.Add(PieceType.ROOK, "rook");
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var piece = (Piece)value;
            if (piece == null)
                return null;
           var source = Path.GetFullPath(@"..\..\Resources\chess_piece_"+PIECE_COLOR[piece.Player] + "_" + PIECE_TYPE[piece.PieceType] + "_T.png");
        
            return source;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
