using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ChessProjectFinal.Common;
using ChessProjectFinal.Entities;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ViewModel
{
    public class BoardEditorViewModel : BasePropertyChanged
    {

        #region CONSTRUCTOR
        public BoardEditorViewModel(Board board)
        {
            BlackPieces = new ObservableCollection<Piece> { Piece.BLACK_BISHOP, Piece.BLACK_ROOK, Piece.BLACK_QUEEN, Piece.BLACK_KNIGHT, Piece.BLACK_PAWN, Piece.BLACK_KING };
            WhitePieces = new ObservableCollection<Piece> { Piece.WHITE_BISHOP, Piece.WHITE_ROOK, Piece.WHITE_QUEEN, Piece.WHITE_KNIGHT, Piece.WHITE_PAWN, Piece.WHITE_KING };
            IsActive = true;
            PlayerColor = Colors.White;
            this.board = board;
        }
        #endregion

        #region PRIVATE FIELDS
        private readonly Board board;
        #endregion

        #region PRIVATE BACKING FIELDS
        private ObservableCollection<Piece> whitePieces;
        private ObservableCollection<Piece> blackPieces;
        private bool whiteCastleKingSide;
        private bool whiteCastleQueenSide;
        private bool blackCastleKingSide;
        private bool blackCastleQueenSide;
        private DelegateCommand<Piece> selectPieceCommand;
        private bool isActive;
        private Color playerColor;
        private Piece selectedPiece;
        private ICommand switchColorCommand;
        private DelegateCommand<Square> emptySquareCommand;

        private DelegateCommand<Square> clickSquareCommand;

        public DelegateCommand<Square> EmptySquareCommand
        {
            get
            {
                if (emptySquareCommand == null)
                    emptySquareCommand = new DelegateCommand<Square>(EmptySquare);
                return emptySquareCommand;
            }
        } 
        #endregion

        #region PRIVATE METHODS
        private void placePiece(Square square)
        {
            square.Occupant = selectedPiece;
        }

        private void switchColor(object obj)
        {
            if (PlayerColor == Colors.White)
                PlayerColor = Colors.Black;
            else
                PlayerColor = Colors.White;

        }
        private void selectPiece(Piece piece)
        {
            selectedPiece = piece;
        }
        private void EmptySquare(Square square)
        {
            square.Occupant = null;
        }

        #endregion

        #region PUBLIC PROPERTIES
        public ObservableCollection<Piece> BlackPieces
        {
            get { return blackPieces; }
            set
            {
                blackPieces = value;
                RaisePropertyChanged(() => BlackPieces);
            }
        }
        public ObservableCollection<Piece> WhitePieces
        {
            get { return whitePieces; }
            set
            {
                whitePieces = value;
                RaisePropertyChanged(() => WhitePieces);
            }
        }

        public bool WhiteCastleKingSide
        {
            get { return whiteCastleKingSide; }
            set
            {
                whiteCastleKingSide = value;
                RaisePropertyChanged(() => WhiteCastleKingSide);
            }
        }
        public bool BlackCastleKingSide
        {
            get { return blackCastleKingSide; }
            set
            {
                blackCastleKingSide = value;
                RaisePropertyChanged(() => BlackCastleKingSide);
            }
        }

        public bool WhiteCastleQueenSide
        {
            get { return whiteCastleQueenSide; }
            set
            {
                whiteCastleQueenSide = value;
                RaisePropertyChanged(() => WhiteCastleQueenSide);
            }
        }
        public bool BlackCastleQueenSide
        {
            get { return blackCastleQueenSide; }
            set
            {
                blackCastleQueenSide = value;
                RaisePropertyChanged(() => BlackCastleQueenSide);
            }
        }
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                RaisePropertyChanged(() => IsActive);
            }

        }
        public Color PlayerColor
        {
            get { return playerColor; }
            set
            {
                playerColor = value;
                RaisePropertyChanged(() => PlayerColor);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public BoardState GetState()
        {
            var board = new Piece[8, 8];
            for (var i = 0; i < 8; i++)
                for (var j = 0; j < 8; j++)
                    board[i, j] = this.board.IndexedSquares[new Point(i, j)].Occupant;
            var queenSide = new CastlingRights(whiteCastleQueenSide, blackCastleQueenSide);
            var kingSide = new CastlingRights(whiteCastleKingSide, blackCastleKingSide);
            var player = PlayerColor == Colors.Black ? Player.BLACK : Player.WHITE;
            return new BoardState(board, false, new Point(0, 0), queenSide, kingSide, player);
        }
        #endregion

        #region COMMANDS
        public DelegateCommand<Square> ClickSquareCommand
        {
            get
            {
                if (clickSquareCommand == null)
                    clickSquareCommand = new DelegateCommand<Square>(placePiece);
                return clickSquareCommand;
            }
        }
        public DelegateCommand<Piece> SelectPieceCommand
        {
            get
            {
                if (selectPieceCommand == null)
                    selectPieceCommand = new DelegateCommand<Piece>(selectPiece);
                return selectPieceCommand;
            }
        }
        
        public ICommand SwitchColorCommand
        {

            get
            {
                if (switchColorCommand == null)
                    switchColorCommand = new RelayCommand(switchColor);
                return switchColorCommand;
            }
        }
        #endregion





        

        
       


       
       
      
       

        
    }
}
