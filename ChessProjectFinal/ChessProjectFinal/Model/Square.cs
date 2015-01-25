using System.Windows;
using ChessProjectFinal.Common;

namespace ChessProjectFinal.Model
{
    public class Square :BasePropertyChanged, ISquare
    {
        private IPiece occupant;
        private int row;
        private int col;
        private bool isValidMove;

        public Square()
        {
        }
        public Square(int row, int column) : this()
        {
            Row = row;
            Column = column;
        }
        public IPiece Occupant
        {
            get
            {
                return this.occupant;
            }
            set
            {
                occupant = value;
                RaisePropertyChanged(() => Occupant);

            }
        }

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
    }
}
