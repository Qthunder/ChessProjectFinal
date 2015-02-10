using System.Collections.Generic;


namespace ChessProjectFinal.Model
{
    public interface IPiece
    {
        
       
        ISquare Location { get; set; }
        PieceStruct PieceStruct { get; set; }

        ISet<ISquare> MoveSquares { get; set; }

        bool IsMoveable { get; }

        bool IsActive { get; set; }
    }
}
