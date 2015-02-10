using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using ChessProjectFinal.Common;

namespace ChessProjectFinal.Model
{
      public class Board : BasePropertyChanged,IBoard
    {
        static Point Point(int i, int j)
        {
            return new Point(i, j);

        }
        private ObservableCollection<IPiece> pieces;

        private ObservableCollection<ISquare> squares;

        private Dictionary<Point, ISquare> indexedSquares;

        private IPiece whiteKing;

        private IPiece blackKing;

        private IPiece selectedPiece;

        public void MakeMove(IPiece piece, ISquare targetSquare)
        {

            if (targetSquare.IsOccupied)
            {
                var capturedPiece = targetSquare.Occupant;
                Pieces.Remove(capturedPiece); //TODO captured piece storage
            }
            piece.Location.Occupant = null; //Empty Piece TODO
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
                this.InitSquares();
            }
        }

        public IPiece WhiteKing
        {
            get
            {
                return whiteKing;
            }
            set
            {
                whiteKing = value;
                RaisePropertyChanged(() => WhiteKing);
            }
        }

        public IPiece BlackKing
        {
            get
            {
                return blackKing;
            }
            set
            {
                blackKing = value;
                RaisePropertyChanged(() => BlackKing);
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

        public Dictionary<Player, IPiece> Kings
        {
            get
            {
                return new Dictionary<Player, IPiece>
                {
            {Player.White, WhiteKing},
            {Player.Black,BlackKing}
            
            };
            }

        }

        public void Initialize()
        {
            Squares = new ObservableCollection<ISquare>();
            Pieces = new ObservableCollection<IPiece>();
            for (int i = 0; i < Game.BOARD_SIZE; i++)
                for (int j = 0; j < Game.BOARD_SIZE; j++)
                    Squares.Add(new Square(i, j));
            this.InitSquares();
            foreach (Player player in Enum.GetValues(typeof(Player)))
            {
                for (int i = 0; i < Game.BOARD_SIZE; i++)
                {
                    AddPiece(new Piece(PieceType.Pawn, player), Game.PAWN_ROW(player), i);
                }

                AddPiece(new Piece(PieceType.Rook,player), Game.BASE_ROW(player), 0 );
                AddPiece(new Piece(PieceType.Rook, player), Game.BASE_ROW(player), 7);
                AddPiece(new Piece(PieceType.Knight, player), Game.BASE_ROW(player), 1);
                AddPiece(new Piece(PieceType.Knight, player), Game.BASE_ROW(player), 6);
                AddPiece(new Piece(PieceType.Bishop, player), Game.BASE_ROW(player), 2);
                AddPiece(new Piece(PieceType.Bishop, player), Game.BASE_ROW(player), 5);
                AddPiece(new Piece(PieceType.Queen, player), Game.BASE_ROW(player), 3 );
                AddPiece(new Piece(PieceType.King, player), Game.BASE_ROW(player), 4 );

            }

            RaisePropertyChanged(() => Squares);
            RaisePropertyChanged(() => Pieces);

        }

        private void AddPiece(IPiece piece, int i, int j)
        {
            var point = new Point(i, j);
            piece.Location = IndexedSquares[point];
            piece.Location.Occupant = piece;
            Pieces.Add(piece);
            RaisePropertyChanged(() => Pieces);

        }

        public void InitSquares()
        {
            IndexedSquares = new Dictionary<Point, ISquare>();
            foreach (ISquare square in Squares)
            {
                IndexedSquares.Add(new Point(square.Row, square.Column), square);
            }
            this.RaisePropertyChanged(()=> Squares);
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




   }
}
