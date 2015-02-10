using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ChessProjectFinal.Common;

namespace ChessProjectFinal.Model
{
      public class Board : BasePropertyChanged,IBoard
    {

        #region PRIVATE BACKING FIELDS

        private ObservableCollection<IPiece> pieces;

        private ObservableCollection<ISquare> squares;

        private Dictionary<Point, ISquare> indexedSquares;

        private IPiece selectedPiece;

        #endregion

        #region PROPERTIES
        public ObservableCollection<IPiece> Pieces
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
        public IPiece SelectedPiece
        {
            get
            {
                return selectedPiece;
            }
            set
            {
                selectedPiece = value;
                RaisePropertyChanged(() => SelectedPiece);
                if (SelectedPiece != null)
                    SetValidMoves(SelectedPiece);
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
        public void MakeMove(IPiece piece, ISquare targetSquare)
        {

            if (targetSquare.IsOccupied)
            {
                var capturedPiece = targetSquare.Occupant;
                Pieces.Remove(capturedPiece); 
            }
            piece.Location.Occupant = null; 
            targetSquare.Occupant = piece;
            piece.Location = targetSquare;

            ResetValidMoves();

            RaisePropertyChanged(() => Pieces);


        }
        public void SetValidMoves(IPiece currentSelectedPiece)
        {
            ResetValidMoves();

            foreach (ISquare square in currentSelectedPiece.MoveSquares)
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
            Squares = new ObservableCollection<ISquare>();
            Pieces = new ObservableCollection<IPiece>();
            IndexedSquares = new Dictionary<Point, ISquare>();
            for (int i = 0; i < Game.BOARD_SIZE; i++)
                for (int j = 0; j < Game.BOARD_SIZE; j++)
                {
                    var newSquare = new Square(i, j);
                    Squares.Add(newSquare);
                    IndexedSquares.Add(new Point(i,j),newSquare );
                }

        }
        #endregion
   }
}
