using System.Collections.Generic;
using System.Windows;
using ChessProjectFinal.ChessSearch;
using ChessProjectFinal.Common;
using ChessProjectFinal.Search;
using ChessProjectFinal.ViewModel;

namespace ChessProjectFinal.Model
{
    public class Game :BasePropertyChanged
    {
        #region CONSTRUCTOR
        public Game()
        {
            WhitePlayer = PlayerType.Human;
            BlackPlayer = PlayerType.AI;
            playerSearches[Player.Black]=new NegaMaxSearch(new BasicHeuristic(), new NoOrdering());
            playerSearches[Player.White] = new NegaMaxSearch(new BasicHeuristic(), new NoOrdering());

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
        private Board board;
        private IInternalBoard internalBoard;
        private Player currentPlayer;
        private bool isActive;
        private bool isBusy;
        private readonly Dictionary<Player,PlayerType> playerTypes=new Dictionary<Player,PlayerType>(); 
        private readonly Dictionary<Player,ISearch> playerSearches=new Dictionary<Player,ISearch>(); 
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
            var move = createMove(Board.SelectedSquare, targetSquare);
            InternalBoard.DoMove(move);
            swapPlayer();
            ReSync();
            checkAI();

          
        }
       
        public void Restart()
        {
            Board = new Board();
            InternalBoard = new GameHistory();
            Board.Initialize();
            CurrentPlayer = Player.White;
            ReSync();
            IsActive = true;
            this.checkAI();

        }

        public PlayerType WhitePlayer
        {
            get { return playerTypes[Player.White]; }

            set
            {
                playerTypes.Add(Player.White, value);
                RaisePropertyChanged(() => WhitePlayer);
            }
        }

        public PlayerType BlackPlayer
        {
            get { return playerTypes[Player.Black]; }

            set
            {
                playerTypes[Player.Black] = value;
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
                square.Occupant = state.PieceBoard[square.Row, square.Column];
                square.MoveSquares.Clear();
                Board.Pieces.Add(square.Occupant); 
            }
            foreach (var move in moves)
            {
                Board.IndexedSquares[move.From].MoveSquares.Add(Board.IndexedSquares[move.To]);
            }
        }

        private Move createMove(ISquare fromSquare, ISquare targetSquare)
        {
            var capturedPiece = targetSquare.Occupant;
            var piece = fromSquare.Occupant;
            if (piece.PieceType == PieceType.Pawn &&
                targetSquare.Row == BASE_ROW(OTHER_PLAYER(piece.Player)))
            {
                var promotionPiece = this.promoteDialog();
                return new Move(fromSquare.Coords,targetSquare.Coords,piece,capturedPiece){Promotion = promotionPiece};
            }
            if (piece.PieceType == PieceType.Pawn && fromSquare.Column != targetSquare.Column &&  targetSquare.Occupant==null ) 
                return new Move(fromSquare.Coords,targetSquare.Coords,piece, Board.IndexedSquares[new Point(targetSquare.Column,fromSquare.Row)].Occupant){IsEnPassant = true};
            if (piece.PieceType==PieceType.King && fromSquare.Column+2==targetSquare.Column)
                return new Move(fromSquare.Coords,targetSquare.Coords,piece,null){IsKingSideCastle = true};
            if (piece.PieceType == PieceType.King &&fromSquare.Column - 2 == targetSquare.Column)
                return new Move(fromSquare.Coords, targetSquare.Coords, piece, null) { IsQueenSideCastle = true };

            return new Move(fromSquare.Coords, targetSquare.Coords, piece, capturedPiece);
        }

        private Piece promoteDialog()
        {
            var viewModel = new PromotionViewModel(CurrentPlayer);
            return viewModel.PieceStruct;
        }

     

        

        private void swapPlayer()
        {
            CurrentPlayer = OTHER_PLAYER(CurrentPlayer);
            Board.SelectedSquare = null;
        }
        
        private void checkAI()
        {
            var side = CurrentPlayer == Player.White ? 1 : -1;
            if (this.playerTypes[this.CurrentPlayer] != PlayerType.AI) return;
            var result = this.playerSearches[this.CurrentPlayer].SearchByDepth(new Node(new AIState(InternalBoard.GetState()), null), 2,side);
            var moveToMake = (Move) result.Item1;
            InternalBoard.DoMove(moveToMake);
            swapPlayer();
            ReSync();
            checkAI();
        }
            #endregion


    }
}
