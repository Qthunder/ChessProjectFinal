using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ChessProjectFinal.ChessSearch;
using ChessProjectFinal.Common;
using ChessProjectFinal.Entities;
using ChessProjectFinal.ViewModel;

namespace ChessProjectFinal.Model
{
    public class Game :BasePropertyChanged
    {
        #region CONSTRUCTOR
        public Game()
        {
            PlayerSearches[Player.BLACK]= new NegaMaxSearchChess(new EvaluationFunction());
            PlayerSearches[Player.WHITE] =new NegaMaxSearchChess(new EvaluationFunction());
            Board = new Board();
            GameHistory = new GameHistory();
            ReSync();
            IsActive = false;
        }
        #endregion
        #region STATIC HELPER FUNCTIONS
        public static Player OTHER_PLAYER(Player player)
        {
            return player == Player.WHITE ? Player.BLACK : Player.WHITE;
        }
        public const int BOARD_SIZE = 8;
        public static int BASE_ROW(Player player)


        {
            return player == Player.WHITE ? 0 : 7;
        }
        public static int PAWN_ROW(Player player)
        {
            return player == Player.WHITE ? 1 : 6;
        }
     
        
        #endregion
        #region PRIVATE BACKING FIELDS
        private Board board;
        private GameHistory gameHistory;
        
        private bool isActive;
        private bool isBusy;

     
        private readonly Dictionary<Player,PlayerType> playerTypes=new Dictionary<Player,PlayerType>(); 
        public readonly Dictionary<Player,NegaMaxSearchChess> PlayerSearches=new Dictionary<Player,NegaMaxSearchChess>(); 
        #endregion
        #region PROPERTIES 

        public NegaMaxSearchChess WhiteSearch
        {
            get { return PlayerSearches[Player.WHITE]; }
        }
        public NegaMaxSearchChess BlackSearch
        {
            get { return PlayerSearches[Player.BLACK]; }
        }
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
        public bool IsActive { get { return isActive; } set { isActive = value; RaisePropertyChanged(()=>IsActive);} }
        public bool IsBusy { get { return isBusy; } set { isBusy = value; RaisePropertyChanged(()=>IsBusy); } }
        public PlayerType WhitePlayer
        {
            get { return playerTypes[Player.WHITE]; }

            set
            {
                playerTypes[Player.WHITE]= value;
                RaisePropertyChanged(() => WhitePlayer);
            }
        }
        public PlayerType BlackPlayer
        {
            get { return playerTypes[Player.BLACK]; }

            set
            {
                playerTypes[Player.BLACK] = value;
                RaisePropertyChanged(() => BlackPlayer);
            }
        }
        
        #endregion
        #region PRIVATE FIELDS

        private CancellationTokenSource stopGameCancellationTokenSource=new CancellationTokenSource();
        private Task workTask = Task.Factory.StartNew(() => { });

        #endregion
        #region PUBLIC METHODS
        public void MovePiece(ISquare targetSquare)
        {
            var move = createMove(Board.SelectedSquare, targetSquare);
            GameHistory.MakeMove(move);
            ReSync();
            movePieceSound();
            Board.SelectedSquare = null;
            AI();
        }
       
        public void StartGame()
        {
            if (IsActive)
                StopGame();
            IsActive = true;
            GameHistory = new GameHistory();
            IsActive = true;
            ReSync();
            AI();


        }

        public void ForceStopGame()
        {
            stopGameCancellationTokenSource.Cancel();
            workTask.Wait();
            StopGame();
        }

        public void StopGame()
        {
            stopGameCancellationTokenSource=new CancellationTokenSource();
            IsActive = false;
        }

        public void EndGame()
        {
            stopGameCancellationTokenSource.Cancel();
            victorySound();
            StopGame();
        }

       


        #endregion
        #region PRIVATE METHODS
        
        public void ReSync()
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
          
            if (BoardState.IsCheckMate(GameHistory.CurrentState, GameHistory.CurrentState.CurrentPlayer) || BoardState.IsStaleMate(GameHistory.CurrentState,GameHistory.CurrentState.CurrentPlayer))
                EndGame();
        }

        private Move createMove(ISquare fromSquare, ISquare targetSquare)
        {
            var capturedPiece = targetSquare.Occupant;
            var piece = fromSquare.Occupant;
            if (piece.PieceType == PieceType.PAWN &&
                targetSquare.Row == BASE_ROW(OTHER_PLAYER(piece.Player)))
            {
                var promotionPiece = promoteDialog();
                return new Move(fromSquare.Coords, targetSquare.Coords, piece, capturedPiece, false, false, false,
                    promotionPiece);
            }
            if (piece.PieceType == PieceType.PAWN && fromSquare.Column != targetSquare.Column &&  targetSquare.Occupant==null ) 
                return new Move(fromSquare.Coords,targetSquare.Coords,piece, Board.IndexedSquares[new Point(targetSquare.Column,fromSquare.Row)].Occupant,true,false,false,null);
            if (piece.PieceType == PieceType.KING && fromSquare.Column + 2 == targetSquare.Column)
                return new Move(fromSquare.Coords, targetSquare.Coords, piece, null, false, true, false, null);
            if (piece.PieceType == PieceType.KING && fromSquare.Column - 2 == targetSquare.Column)
                return new Move(fromSquare.Coords, targetSquare.Coords, piece, null, false, false, true, null);

            return new Move(fromSquare.Coords, targetSquare.Coords, piece, capturedPiece);
        }

        private Piece promoteDialog()
        {
            var viewModel = new PromotionViewModel(GameHistory.CurrentState.CurrentPlayer);
            return viewModel.PieceStruct;
        }

        private void movePieceSound()
        {
            new Thread(()=>
            {
                var path = new Uri(Path.GetFullPath(@"..\..\Resources\MoveSound.wav"));
                var soundPlayer = new MediaPlayer();
                soundPlayer.Open(path);
                soundPlayer.Play();
                Thread.Sleep(1000);
            }
                ).Start();
        }
        private void victorySound()
        {
            new Thread(() =>
            {
                var path = new Uri(Path.GetFullPath(@"..\..\Resources\VictorySound.wav"));
                var soundPlayer = new MediaPlayer();
                soundPlayer.Open(path);
                soundPlayer.Play();
                Thread.Sleep(1000);
            }
                ).Start();
        }






     /*   public async void AI()
        {
            if (!IsActive) return;
            IsBusy = true;
            if (playerTypes[GameHistory.CurrentState.CurrentPlayer] != PlayerType.AI)
            {
                IsBusy = false;
                return;

            }
            var task =
                PlayerSearches[GameHistory.CurrentState.CurrentPlayer].Search(
                    GameHistory.CurrentState.CurrentPlayer, GameHistory.CurrentState);
           
            GameHistory.MakeMove(await task);
            ReSync();
            movePieceSound();
            IsBusy = false;
        }*/

        public void AI()
        {
            IsBusy = true;
            var token = stopGameCancellationTokenSource.Token;
            workTask = new Task(() => aiWork(token), token);
            workTask.Start();
            workTask.ContinueWith(task => { IsBusy = false; });
            
        }

        private void aiWork(CancellationToken token)
        {


            while (playerTypes[GameHistory.CurrentState.CurrentPlayer] == PlayerType.AI)
            {
               
                
                var move =PlayerSearches[GameHistory.CurrentState.CurrentPlayer].Search(GameHistory.CurrentState.CurrentPlayer, GameHistory.CurrentState);
                if (token.IsCancellationRequested)
                    return;
                if (move != null)
                {
                    GameHistory.MakeMove(move);
                }
               
                ReSync();
                movePieceSound();
            }
        }

        #endregion


    }
}
