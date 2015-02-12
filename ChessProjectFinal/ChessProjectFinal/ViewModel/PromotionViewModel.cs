using System;
using ChessProjectFinal.Common;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ViewModel
{
    public class PromotionViewModel :BasePropertyChanged
    {
        public Action CloseAction { get; set; }

        private Piece pieceStruct;
        private readonly Player player;
        public Piece PieceStruct
        {
            get
            {
                return pieceStruct; 
                
            }
            set
            {
                pieceStruct = value;
                this.RaisePropertyChanged(()=>PieceStruct);
            }
        }


        private DelegateCommand<object> bishopCommand;
        private DelegateCommand<object> knightCommand;
        private DelegateCommand<object> rookCommand;
        private DelegateCommand<object> queenCommand;

        public DelegateCommand<object> BishopCommand
        {
            get
            {
                if (this.bishopCommand == null)
                    this.bishopCommand = new DelegateCommand<object>(this.BishopPick);
                return bishopCommand;
            }

        }
        public DelegateCommand<object> KnightCommand
        {
            get
            {
                if (this.knightCommand == null)
                    this.knightCommand = new DelegateCommand<object>(this.KnightPick);
                return knightCommand;
            }

        }
        public DelegateCommand<object> RookCommand
        {
            get
            {
                if (this.rookCommand == null)
                    this.rookCommand = new DelegateCommand<object>(this.RookPick);
                return rookCommand;
            }

        }
        public DelegateCommand<object> QueenCommand
        {
            get
            {
                if (this.queenCommand == null)
                    this.queenCommand = new DelegateCommand<object>(this.QueenPick);
                return queenCommand;
            }

        }

        public void BishopPick(object context)
        {
            PieceStruct = Piece.PIECES[player][PieceType.Bishop];
            CloseAction();
        }
        public void KnightPick(object context)
        {
            PieceStruct = Piece.PIECES[player][PieceType.Knight];
            CloseAction();
        }
        public void RookPick(object context)
        {
            PieceStruct = Piece.PIECES[player][PieceType.Rook];
            CloseAction();
        }
        public void QueenPick(object context)
        {
            PieceStruct = Piece.PIECES[player][PieceType.Queen];
            CloseAction();
        }

        public PromotionViewModel(Player player)
        {
            this.player = player;
            var view = new View.PromotionView {DataContext = this};
            CloseAction=view.Close;
            view.ShowDialog();
       }




    }
}
