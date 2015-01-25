using System;

namespace ChessProjectFinal.Model
{
    public interface IMove
    {
        Boolean IsEnPassant { get; set; }
        Boolean IsKingSideCastle{get; set; }
        Boolean IsQueenSideCastle { get; set; }
        PieceStruct Piece { get; set; }
        PieceStruct CapturedPiece { get; set; }
        PieceStruct Promotion { get; set; }
        System.Windows.Point From { get; set; }
        System.Windows.Point To { get; set; }
        
        


    }
}
