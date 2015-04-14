﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
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
            WhitePlayer = PlayerType.HUMAN;
            BlackPlayer = PlayerType.HUMAN;
            playerSearches[Player.BLACK]= new NegaMaxSearchChess(new EvaluationFunction());
            playerSearches[Player.WHITE] =new NegaMaxSearchChess(new EvaluationFunction());
            Board = new Board();
            Start();
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
        public static MediaPlayer SoundPlayer= new MediaPlayer();
        
        #endregion
        #region PRIVATE BACKING FIELDS
        private Board board;
        private GameHistory gameHistory;
        
        private bool isActive;
        private bool isBusy;

     
        private readonly Dictionary<Player,PlayerType> playerTypes=new Dictionary<Player,PlayerType>(); 
        private readonly Dictionary<Player,NegaMaxSearchChess> playerSearches=new Dictionary<Player,NegaMaxSearchChess>(); 
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
            ReSync();
            Board.SelectedSquare = null;
            if (IsActive)
               new Thread(CheckAI).Start();
            
        }
       
        public void Start()
        {
            
            GameHistory = new GameHistory();
            IsActive = true;
            ReSync();
            new Thread(CheckAI).Start(); 

        }

        public void EndGame()
        {
            IsActive = false;
            var path = new Uri(Path.GetFullPath(@"..\..\Resources\VictorySound.wav"));
            SoundPlayer.Open(path);
            SoundPlayer.Play();
        }

        public PlayerType WhitePlayer
        {
            get { return playerTypes[Player.WHITE]; }

            set
            {
                playerTypes.Add(Player.WHITE, value);
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
            var path = new Uri(Path.GetFullPath(@"..\..\Resources\MoveSound.wav"));
            SoundPlayer.Open(path);
            SoundPlayer.Play();
            if (BoardState.IsCheckMate(GameHistory.CurrentState, GameHistory.CurrentState.CurrentPlayer) ||BoardState.IsStaleMate(GameHistory.CurrentState,GameHistory.CurrentState.CurrentPlayer))
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

     

        

       
        
        public async void CheckAI()
        {
            IsBusy = true;
            if (playerTypes[GameHistory.CurrentState.CurrentPlayer] != PlayerType.AI)
            {
                IsBusy = false;
                return;
                
            }
            var task = playerSearches[GameHistory.CurrentState.CurrentPlayer].Search(GameHistory.CurrentState.CurrentPlayer,GameHistory.CurrentState,6,1000000);
            GameHistory.MakeMove(await task);
            ReSync();
            IsBusy = false;
           
        }
            #endregion


    }
}
