using System.Collections.Generic;


namespace ChessProjectFinal.Model
{
    public interface IPiece
    {
        PieceStruct PieceStruct { get; set; } 
       
        ISquare Location { get; set; }

        ISet<ISquare> MoveSquares { get; set; }

        bool IsMoveable { get; }

        bool IsActive { get; set; }
    }
}
