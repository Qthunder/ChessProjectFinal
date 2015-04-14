using System;
using System.Collections.ObjectModel;
using ChessProjectFinal.Common;
using ChessProjectFinal.Entities;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ViewModel
{
    public class PromotionViewModel :BasePropertyChanged
    {
        private ObservableCollection<Piece> promotionPieces;
        public Action CloseAction { get; set; }

        private Piece pieceStruct;
       
        public Piece PieceStruct
        {
            get
            {
                return pieceStruct; 
                
            }
            set
            {
                pieceStruct = value;
                RaisePropertyChanged(()=>PieceStruct);
            }
        }

        public ObservableCollection<Piece> PromotionPieces
        {
            get { return promotionPieces; }
            set
            {
                promotionPieces = value;
                RaisePropertyChanged(()=>PromotionPieces);
            }
            
        }

        public DelegateCommand<Piece> ChoosePieceCommand
        {
            get { return choosePieceCommand ?? (choosePieceCommand = new DelegateCommand<Piece>(choosePiece)); }
        }

        private void choosePiece(Piece selectedPiece)
        {
            PieceStruct = selectedPiece;
            CloseAction();
        }

       private DelegateCommand<Piece> choosePieceCommand;


        public PromotionViewModel(Player player)
        {
            
            var pieces = Piece.PIECES[player];
            var choices = new[]{pieces[PieceType.QUEEN], pieces[PieceType.ROOK], pieces[PieceType.BISHOP], pieces[PieceType.KNIGHT]};
            PromotionPieces= new ObservableCollection<Piece>(choices);
            var view = new View.PromotionView {DataContext = this};
            CloseAction=view.Close;
            view.ShowDialog();
       }




    }
}
