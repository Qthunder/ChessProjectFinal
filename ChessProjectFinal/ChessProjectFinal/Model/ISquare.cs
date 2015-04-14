using System.Collections.Generic;
using ChessProjectFinal.Entities;

namespace ChessProjectFinal.Model
{
    public interface ISquare
    {
        Piece Occupant { get; set; }

        bool IsOccupied { get; }

        int Row { get; set; }

        int Column { get; set; }

        System.Windows.Point Coords { get; }

        bool IsValidMove { get; set; }

        ISet<ISquare> MoveSquares { get; set; }

        bool IsMoveable { get; }

        bool IsActive { get; set; }

    }
}
