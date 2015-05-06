using System;
using System.Windows.Input;
using System.Windows.Media;
using ChessProjectFinal.Common;
using ChessProjectFinal.Entities;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ViewModel
{
    public class BoardViewModel : BasePropertyChanged
    {

        #region Constructor
       
        public BoardViewModel()
        {
            Game = new Game();
            BoardEditorViewModel = new BoardEditorViewModel(Game.Board);
        }
        #endregion
        #region PRIVATE BACKING FIELDS
        private Game game;
        private Boolean editorMode;
        private BoardEditorViewModel boardEditorViewModel;
        private DelegateCommand<Square> makeMoveCommand;
        private ICommand editModeCommand;
        private ICommand restartCommand;
        private ICommand saveChangesCommand;
        private DelegateCommand<Square> activatePieceCommand;
        private ICommand newGameCommand;
        private ICommand stopCommand;

        #endregion
        #region PUBLIC PROPERTIES
        public Boolean EditorMode
        {
            get { return editorMode; }
            set
            {
                editorMode = value;
                RaisePropertyChanged(() => EditorMode);
            }
        }
        public BoardEditorViewModel BoardEditorViewModel
        {
            get { return boardEditorViewModel; }
            set
            {
                boardEditorViewModel = value;
                RaisePropertyChanged(() => BoardEditorViewModel);
            }
        }
        public Game Game
        {
            get { return game; }
            set
            {
                game = value;
                RaisePropertyChanged(() => Game);
            }
        }
        
#endregion
        #region COMMANDS
        public DelegateCommand<Square> ActivatePieceCommand
        {
            get
            {
                if (activatePieceCommand == null)
                    activatePieceCommand = new DelegateCommand<Square>(activatePiece);
                return activatePieceCommand;
            }
        }
        public DelegateCommand<Square> MakeMoveCommand
        {
            get
            {
                if (makeMoveCommand == null)
                    makeMoveCommand = new DelegateCommand<Square>(makeMove);
                return makeMoveCommand;
            }
        }
        public ICommand EditModeCommand
        {
            get
            {
                if (editModeCommand == null)
                    editModeCommand = new RelayCommand(changeEditMode);
                return editModeCommand;

            }
         
        }
        public ICommand RestartCommand
        {

            get
            {
                if (restartCommand == null)
                    restartCommand = new RelayCommand(restart);
                return restartCommand;
            }
        }
        public ICommand StopCommand
        {

            get
            {
                if (stopCommand == null)
                    stopCommand = new RelayCommand(stop);
                return stopCommand;
            }
        }

       

        public ICommand SaveChangesCommand
        {

            get
            {
                if (saveChangesCommand == null)
                    saveChangesCommand = new RelayCommand(saveChanges);
                return saveChangesCommand;
            }
        }

        public ICommand NewGameCommand
        {
            get
            {
                if (newGameCommand == null)
                    newGameCommand = new RelayCommand(newGame);
                return newGameCommand;
            }
        }
        #endregion
        #region PRIVATE MTHODS
        private void saveChanges(object obj)
        {
            var state = BoardEditorViewModel.GetState();

            Game.GameHistory = new GameHistory(state);
            Game.ReSync();
           


        }
        private void restart(object obj)
        {
            restartGame();
        }
        private void stop(object obj)
        {
           Game.ForceStopGame();
        }
        private void activatePiece(ISquare targetSquare)
        {
            if (!Game.IsActive) return;
            if (targetSquare.IsMoveable && !targetSquare.IsActive)
            {
                targetSquare.IsActive = true;
                Game.Board.SelectedSquare = targetSquare;
            }
            else
            {
                if (!targetSquare.IsMoveable) return;
                targetSquare.IsActive = false;
                Game.Board.SelectedSquare = null;
            }
        }
        private void restartGame()
        {
            Game.StartGame();
        }
        private void makeMove(Square targetSquare)
        {
            if (Game.IsActive)
            {
                Game.MovePiece(targetSquare);
            }
        }
        private void newGame(object obj)
        {
            var viewModel = new NewGameViewModel();
            if (viewModel.IsCanceled) return;
            Game.WhitePlayer = viewModel.WhitePlayerType;
            Game.BlackPlayer = viewModel.BlackPlayerType;
            Game.PlayerSearches[Player.WHITE].NewSettings(viewModel.WhiteUsingPV, viewModel.WhiteDepth, viewModel.WhiteTime);
            Game.PlayerSearches[Player.BLACK].NewSettings(viewModel.BlackUsingPV, viewModel.BlackDepth, viewModel.BlackTime);
             
            Game.StartGame();
        }

        private void changeEditMode(object obj)
        {
            if (EditorMode)
            {
                EditorMode = false;
                Game.ReSync();
                Game.AI();
            }
            else
            {
                EditorMode = true;
                BoardEditorViewModel.BlackCastleKingSide = Game.GameHistory.CurrentState.CastleKingSide[Player.BLACK];
                BoardEditorViewModel.WhiteCastleKingSide = Game.GameHistory.CurrentState.CastleKingSide[Player.WHITE];
                BoardEditorViewModel.BlackCastleQueenSide = Game.GameHistory.CurrentState.CastleKingSide[Player.BLACK];
                BoardEditorViewModel.WhiteCastleQueenSide = Game.GameHistory.CurrentState.CastleKingSide[Player.WHITE];
                BoardEditorViewModel.PlayerColor = Game.GameHistory.CurrentState.CurrentPlayer==Player.WHITE ? Colors.White :Colors.Black;
            }
        }
        #endregion
        
       

       
    }
}
