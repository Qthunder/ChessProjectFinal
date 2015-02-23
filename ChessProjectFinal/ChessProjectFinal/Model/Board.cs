using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ChessProjectFinal.Common;

namespace ChessProjectFinal.Model
{
      public class Board : BasePropertyChanged,IBoard
    {
          public Board()
          {
              Squares = new ObservableCollection<ISquare>();
              Pieces = new ObservableCollection<Piece>();
              IndexedSquares = new Dictionary<Point, ISquare>();
              for (int i = 0; i < Game.BOARD_SIZE; i++)
                  for (int j = 0; j < Game.BOARD_SIZE; j++)
                  {
                      var newSquare = new Square(i, j);
                      Squares.Add(newSquare);
                      IndexedSquares.Add(new Point(i, j), newSquare);
                  }
          }
        #region PRIVATE BACKING FIELDS

        private ObservableCollection<Piece> pieces;

        private ObservableCollection<ISquare> squares;

        private Dictionary<Point, ISquare> indexedSquares;

        private ISquare selectedSquare;

        #endregion

        #region PROPERTIES
        public ObservableCollection<Piece> Pieces
        {
            get
            {
                return pieces;
            }
            set
            {
                if (value != pieces)
                {
                    pieces = value;
                    RaisePropertyChanged(() => Pieces);
                }
            }
        }
        public ObservableCollection<ISquare> Squares
        {
            get
            {
                return squares;
            }
            set
            {
                squares = value;
                RaisePropertyChanged(() => Squares);
            }
        }
        public ISquare SelectedSquare
        {
            get
            {
                return selectedSquare;
            }
            set
            {
                selectedSquare= value;
                RaisePropertyChanged(() => SelectedSquare);
                if (SelectedSquare != null)
                    SetValidMoves(SelectedSquare);
                else
                    ResetValidMoves();
            }
        }
        public Dictionary<Point, ISquare> IndexedSquares
        {
            get
            {
                return indexedSquares;
            }
            set
            {
                indexedSquares = value;
                RaisePropertyChanged(() => IndexedSquares);
            }
        }

        #endregion

        #region METHODS
        public void MakeMove(ISquare fromSquare, ISquare targetSquare)
        {

            if (targetSquare.IsOccupied)
            {
                var capturedPiece = targetSquare.Occupant;
                Pieces.Remove(capturedPiece); 
            }
            targetSquare.Occupant = fromSquare.Occupant;
            fromSquare.Occupant = null;
            ResetValidMoves();

            RaisePropertyChanged(() => Pieces);


        }
        public void SetValidMoves(ISquare currentSelectedSquare)
        {
            ResetValidMoves();

            foreach (var square in currentSelectedSquare.MoveSquares)
            {
                square.IsValidMove = true;
            }


        }
        public void ResetValidMoves()
        {
            foreach (var square in Squares)
            {
                square.IsValidMove = false;
            }
        }
        public void Initialize()
        {
            

        }
        #endregion
   }
}
