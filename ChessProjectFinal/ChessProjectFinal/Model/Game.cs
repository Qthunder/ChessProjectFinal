using System.Collections.Generic;
using System.Threading;
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
            WhitePlayer = PlayerType.AI;
            BlackPlayer = PlayerType.Human;
            playerSearches[Player.Black]=new NegaMaxSearchChess(new EvaluationFunction(), new NoOrdering());
            playerSearches[Player.White] = new NegaMaxSearchChess(new EvaluationFunction(), new NoOrdering());
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
        private GameHistory gameHistory;
        
        private bool isActive;
        private bool isBusy;

     
        private readonly Dictionary<Player,PlayerType> playerTypes=new Dictionary<Player,PlayerType>(); 
        private readonly Dictionary<Player,ISearch> playerSearches=new Dictionary<Player,ISearch>(); 
        #endregion
        #region PROPERTIES 
       
        public GameHistory GameHistory
        {
            get { return gameHistory; }
            set { gameHistory = value; }
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
        public bool IsBusy { get { return isBusy; } set { isBusy = value; RaisePropertyChanged(()=>IsBusy); } }
        #endregion
        #region PUBLIC METHODS
        public void MovePiece(ISquare targetSquare)
        {
            var move = createMove(Board.SelectedSquare, targetSquare);
            GameHistory.MakeMove(move);
            reSync();
            Board.SelectedSquare = null;
            if (IsActive)
               new Thread(checkAI).Start();
            
        }
       
        public void Restart()
        {
            Board = new Board();
            GameHistory = new GameHistory();
            IsActive = true;
            reSync();
            IsActive = true;
            new Thread(checkAI).Start(); 

        }

        public void GameEnd()
        {
            IsActive = false;
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
        
        private void reSync()
        {
            var moves = BoardState.GetValidMoves(GameHistory.CurrentState, GameHistory.CurrentState.CurrentPlayer); 
            var state = GameHistory.CurrentState;
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
            if (BoardState.IsCheckMate(GameHistory.CurrentState, GameHistory.CurrentState.CurrentPlayer) ||BoardState.IsStaleMate(GameHistory.CurrentState,GameHistory.CurrentState.CurrentPlayer))
                GameEnd();
        }

        private Move createMove(ISquare fromSquare, ISquare targetSquare)
        {
            var capturedPiece = targetSquare.Occupant;
            var piece = fromSquare.Occupant;
            if (piece.PieceType == PieceType.Pawn &&
                targetSquare.Row == BASE_ROW(OTHER_PLAYER(piece.Player)))
            {
                var promotionPiece = promoteDialog();
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
            var viewModel = new PromotionViewModel(GameHistory.CurrentState.CurrentPlayer);
            return viewModel.PieceStruct;
        }

     

        

       
        
        private void checkAI()
        {
            IsBusy = true;
            var side = GameHistory.CurrentState.CurrentPlayer == Player.White ? 1 : -1;
            if (playerTypes[GameHistory.CurrentState.CurrentPlayer] != PlayerType.AI)
            {
                IsBusy = false;
                return;
                
            }
            var result = playerSearches[GameHistory.CurrentState.CurrentPlayer].SearchByDepth(new Node(new AIState(GameHistory.CurrentState),null,null), 2,side);
            var moveToMake = (Move) result;
               GameHistory.MakeMove(moveToMake);
            reSync();
            IsBusy = false;
           
        }
            #endregion


    }
}
