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

        private DelegateCommand<Piece> activatePieceCommand;

        private DelegateCommand<Square> makeMoveCommand;

        public DelegateCommand<Piece> ActivatePieceCommand
        {
            get
            {
                if (this.activatePieceCommand == null)
                    this.activatePieceCommand = new DelegateCommand<Piece>(ActivatePiece);
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

        public void ActivatePiece(Piece targetPiece)
        {
            if (!this.Game.IsActive) return;
            if (targetPiece.IsMoveable && !targetPiece.IsActive)
            {
                targetPiece.IsActive = true;
                this.Game.Board.SelectedPiece = targetPiece;
            }
            else
            {
                if (!targetPiece.IsMoveable) return;
                targetPiece.IsActive = false;
                this.Game.Board.SelectedPiece = null;
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
