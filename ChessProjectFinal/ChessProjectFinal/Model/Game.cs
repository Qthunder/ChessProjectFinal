using System.Text.RegularExpressions;
using System.Windows;
using ChessProjectFinal.Common;
using ChessProjectFinal.ViewModel;

namespace ChessProjectFinal.Model
{
    public class Game :BasePropertyChanged
    {
        #region CONSTRUCTOR
        public Game()
        {
            WhitePlayer = PlayerType.Human;
            BlackPlayer = PlayerType.Human;

            Restart();
        }
        #endregion
        #region STATIC HELPER FUNCTIONS
        public static Player OTHER_PLAYER(Player player)
        {
            return player == Player.White ? Player.Black : Player.White;
        }
        public const int BOARD_SIZE = 8;
        public static int BASE_ROW(Player player)


        {
            return player == Player.White ? 0 : 7;
        }
        public static int PAWN_ROW(Player player)
        {
            return player == Player.White ? 1 : 6;
        }
        #endregion
        #region PRIVATE BACKING FIELDS
        private PlayerType whitePlayer;
        private PlayerType blackPlayer;
        private Board board;
        private IInternalBoard internalBoard;
        private Player currentPlayer;
        private bool isActive;
        private bool isBusy;
        #endregion

        #region PROPERTIES 
  
        public Player CurrentPlayer
        {
            get { return currentPlayer; }
            set
            {
                currentPlayer = value;
                RaisePropertyChanged(() => CurrentPlayer);

            }
        }

        public IInternalBoard InternalBoard
        {
            get { return internalBoard; }
            set { internalBoard = value; }
        }
        public Board Board
        {
            get { return board; }

            set
            {
                board = value;
                RaisePropertyChanged(() => Board);
            }

        }
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public bool IsBusy { get { return isBusy; } set { isBusy = value; } }
        #endregion
        #region PUBLIC METHODS
        public void DoMove(Move move)
        {
            
        }
        public void MovePiece(ISquare targetSquare)
        {
            var move = CreateMove(Board.SelectedPiece, targetSquare);
            InternalBoard.DoMove(move);
            SwapPlayer();
            ReSync();
          
        }
        public void Restart()
        {
            Board = new Board();
            InternalBoard = new InternalBoard();
            Board.Initialize();
            CurrentPlayer = Player.White;
            ReSync();
            IsActive = true;
            CheckAI();

        }

        public PlayerType WhitePlayer
        {
            get { return whitePlayer; }

            set
            {
                whitePlayer = value;
                RaisePropertyChanged(() => WhitePlayer);
            }
        }

        public PlayerType BlackPlayer
        {
            get { return blackPlayer; }

            set
            {
                blackPlayer = value;
                RaisePropertyChanged(() => BlackPlayer);
            }
        }

        #endregion
        #region PRIVATE METHODS
        private void ReSync()
        {
            var moves = InternalBoard.GetValidMoves(CurrentPlayer);
            var state = InternalBoard.GetState();
            Board.Pieces.Clear();
            foreach (var square in Board.Squares)
            {
                square.Occupant = new Piece(state.PieceBoard[square.Row, square.Column], square);
                Board.Pieces.Add(square.Occupant); 
            }
            foreach (var move in moves)
            {
                Board.IndexedSquares[move.From].Occupant.MoveSquares.Add(Board.IndexedSquares[move.To]);
                //TODO promotions
                
            }
        }

        private Move CreateMove(IPiece piece, ISquare targetSquare)
        {
            var capturedPiece = (targetSquare.Occupant == null) ? null : targetSquare.Occupant.PieceStruct;
            if (piece.PieceStruct.Piece == PieceType.Pawn &&
                targetSquare.Row == BASE_ROW(OTHER_PLAYER(piece.PieceStruct.Color)))
            {
                var promotionPiece = PromoteDialog();
                return new Move(piece.Location.Coords,targetSquare.Coords,piece.PieceStruct,capturedPiece){Promotion = promotionPiece};
            }
            if (piece.PieceStruct.Piece == PieceType.Pawn && piece.Location.Column != targetSquare.Column &&  (targetSquare.Occupant==null || targetSquare.Occupant.PieceStruct == null )) 
                return new Move(piece.Location.Coords,targetSquare.Coords,piece.PieceStruct, Board.IndexedSquares[new Point(targetSquare.Column,piece.Location.Row)].Occupant.PieceStruct){IsEnPassant = true};
            if (piece.PieceStruct.Piece==PieceType.King && piece.Location.Column+2==targetSquare.Column)
                return new Move(piece.Location.Coords,targetSquare.Coords,piece.PieceStruct,null){IsKingSideCastle = true};
            if (piece.PieceStruct.Piece == PieceType.King && piece.Location.Column - 2 == targetSquare.Column)
                return new Move(piece.Location.Coords, targetSquare.Coords, piece.PieceStruct, null) { IsQueenSideCastle = true };

            return new Move(piece.Location.Coords, targetSquare.Coords, piece.PieceStruct, capturedPiece);
        }

        private PieceStruct PromoteDialog()
        {
            var viewModel = new PromotionViewModel(CurrentPlayer);
            return viewModel.PieceStruct;
        }

     

        

        private void SwapPlayer()
        {
            CurrentPlayer = OTHER_PLAYER(CurrentPlayer);
            Board.SelectedPiece = null;
            CheckAI();
        }

        private void CheckAI()
        {
            //TODO
        }
#endregion


    }
}
