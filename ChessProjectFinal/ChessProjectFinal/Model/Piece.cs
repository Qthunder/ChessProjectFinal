using System.Collections.Generic;
using ChessProjectFinal.Common;

namespace ChessProjectFinal.Model
{
    public class Piece : BasePropertyChanged,IPiece
    {
        

        private ISquare location;
        private PieceStruct pieceStruct;

        private ISet<ISquare> moveSquares = new HashSet<ISquare>();

        private bool isActive;
        public ISquare Location
        {
            get
            {
                return this.location;
            }
            set
            {
                location = value;
            }
        }

        public PieceStruct PieceStruct
        {
            get { return pieceStruct; }
            set
            {
                pieceStruct = value;
                this.RaisePropertyChanged(()=>PieceStruct);
            }
        }

        

        public ISet<ISquare> MoveSquares {
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
            get { return MoveSquares.Count>0; }
        }

        public Piece(PieceType pieceType, Player playerColor)
        {
            PieceStruct = new PieceStruct {Piece = pieceType, Color = playerColor,};
        }

        public Piece(PieceStruct pieceStruct, ISquare location)
        {
            PieceStruct = pieceStruct;
            Location = location;
        }

    }
    
}
