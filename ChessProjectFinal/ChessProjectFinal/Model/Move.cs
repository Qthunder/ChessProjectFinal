using System.Windows;

namespace ChessProjectFinal.Model
{
    public class Move :IMove
    {
        public bool IsEnPassant { get; set; } 
        public bool IsKingSideCastle { get; set; }
        public bool IsQueenSideCastle { get; set; }
        public PieceStruct Piece { get; set; }
        public PieceStruct CapturedPiece { get; set; }
        public PieceStruct Promotion { get; set; }
        public Point From { get; set; }
        public Point To { get; set; }

        public Move() { }
        public Move(Point from, Point to, PieceStruct piece,PieceStruct capturedPiece)
        {
            From = from;
            To = to;
            Piece = piece;
            IsEnPassant = false;
            IsKingSideCastle = false;
            IsQueenSideCastle = false;
            CapturedPiece = capturedPiece;
            Promotion = null;
        }
    }
}
