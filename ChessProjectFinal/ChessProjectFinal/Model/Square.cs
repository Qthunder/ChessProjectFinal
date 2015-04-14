using System.Collections.Generic;
using System.Windows;
using ChessProjectFinal.Common;
using ChessProjectFinal.Entities;

namespace ChessProjectFinal.Model
{
    public class Square :BasePropertyChanged, ISquare
    {

        #region PRIVATE BACKING FIELDS
        private int row;
        private int col;
        private bool isValidMove;
        private Piece pieceStruct;
        private ISet<ISquare> moveSquares = new HashSet<ISquare>();
        private bool isActive;
        #endregion
        #region CONSTRUCTORS
        public Square()
        {
        }
        public Square(int row, int column) : this()
        {
            Row = row;
            Column = column;
        }
        #endregion
        #region PROPERTIES
        public bool IsOccupied
        {
            get
            {
                return (Occupant != null);
            }
        }
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                row = value;
                RaisePropertyChanged(() => Row);
                RaisePropertyChanged(() => Coords);
            }
        }
        public int Column
        {
            get
            {
                return col;
            }
            set
            {
                col = value;
                RaisePropertyChanged(() => Column);
                RaisePropertyChanged(() => Coords);
            }
        }
        public Point Coords
        {
            get { return new Point(Row, Column); }
        }
        public Piece Occupant
        {
            get { return pieceStruct; }
            set
            {
                pieceStruct = value;
                this.RaisePropertyChanged(() => Occupant);
            }
        }
        


        public ISet<ISquare> MoveSquares
        {
            get { return moveSquares; }
            set { moveSquares = value; }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }

            set
            {
                if (value != isActive)
                {
                    isActive = value;
                    RaisePropertyChanged(() => IsActive);
                }
            }
        }

        public bool IsMoveable
        {
            get { return MoveSquares.Count > 0; }
        }

        public bool IsValidMove
        {
            get
            {
                return isValidMove;
            }
            set
            {
                isValidMove = value;
                RaisePropertyChanged(() => IsValidMove);
            }
        }
        #endregion
    }
}
