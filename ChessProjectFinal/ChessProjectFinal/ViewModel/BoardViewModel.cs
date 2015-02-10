using ChessProjectFinal.Common;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ViewModel
{
    public class BoardViewModel : BasePropertyChanged
    {
        private Game game;

        public Game Game
        {
            get { return game; }
            set
            {
                game = value;
                this.RaisePropertyChanged(()=>Game);
            }
        }

        private DelegateCommand<Square> activatePieceCommand;

        private DelegateCommand<Square> makeMoveCommand;

        public DelegateCommand<Square> ActivatePieceCommand
        {
            get
            {
                if (this.activatePieceCommand == null)
                    this.activatePieceCommand = new DelegateCommand<Square>(ActivatePiece);
                return activatePieceCommand;
            }
        }

        public DelegateCommand<Square> MakeMoveCommand
        {
            get
            {
                if (this.makeMoveCommand == null)
                    this.makeMoveCommand = new DelegateCommand<Square>(MakeMove);
                return makeMoveCommand;
            }
        }

        public void ActivatePiece(ISquare targetSquare)
        {
            if (!this.Game.IsActive) return;
            if (targetSquare.IsMoveable && !targetSquare.IsActive)
            {
                targetSquare.IsActive = true;
                this.Game.Board.SelectedSquare = targetSquare;
            }
            else
            {
                if (!targetSquare.IsMoveable) return;
                targetSquare.IsActive = false;
                this.Game.Board.SelectedSquare = null;
            }
        }
       
        public void RestartGame()
        {
            Game.Restart();
        }

        public void MakeMove(Square targetSquare)
        {
            if (Game.IsActive)
            {
                Game.MovePiece(targetSquare);
            }
        }

        public BoardViewModel()
        {
            Game = new Game();

        }
    }
}
