using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ChessProjectFinal.Entities;

namespace ChessProjectFinal.Model
{
    public interface IBoard
    {
        ObservableCollection<Piece> Pieces { get; set; }

        ObservableCollection<ISquare> Squares { get; set; }

        Dictionary<Point, ISquare> IndexedSquares { get; set; }

        void Initialize();

    }
}
