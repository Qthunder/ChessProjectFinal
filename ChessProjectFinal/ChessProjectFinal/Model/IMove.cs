using System;

namespace ChessProjectFinal.Model
{
    public interface IMove
    {
        Boolean IsEnPassant { get; set; }
        Boolean IsKingSideCastle{get; set; }
        Boolean IsQueenSideCastle { get; set; }
        Piece Piece { get; set; }
        Piece CapturedPiece { get; set; }
        Piece Promotion { get; set; }
        System.Windows.Point From { get; set; }
        System.Windows.Point To { get; set; }
        
        


    }
}
