using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace ChessProjectFinal.Model
{
    public interface IBoard
    {
        ObservableCollection<IPiece> Pieces { get; set; }

        ObservableCollection<ISquare> Squares { get; set; }

        Dictionary<Point, ISquare> IndexedSquares { get; set; }

        IPiece WhiteKing { get; set; }

        IPiece BlackKing { get; set; }

        Dictionary<Player, IPiece> Kings { get; }

        void Initialize();

        void InitSquares();
    }
}
