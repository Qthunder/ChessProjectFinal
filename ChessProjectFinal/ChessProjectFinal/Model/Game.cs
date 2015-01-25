using ChessProjectFinal.Common;

namespace ChessProjectFinal.Model
{
    public class Game :BasePropertyChanged
    {
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

        
        private PlayerType whitePlayer;
        private PlayerType blackPlayer;
        private Board board;
        private IInternalBoard internalBoard;
        private Player currentPlayer;
        private bool isActive;
        private bool isBusy;

        public void DoMove(Move move)
        {
            
        }

        public void MovePiece(ISquare targetSquare)
        {
            var move = CreateMove(Board.SelectedPiece, targetSquare);
            InternalBoard.MakeMove(move);
            SwapPlayer();
            ReSync();
          
        }

        private void ReSync()
        {
            var moves = InternalBoard.GetMoves(CurrentPlayer);
            var state = InternalBoard.GetState();
            Board.Pieces.Clear();
            foreach (var square in Board.Squares)
            {
                square.Occupant = new Piece(state[square.Row, square.Column], square);
                Board.Pieces.Add(square.Occupant); 
            }
            foreach (var move in moves)
            {
                Board.IndexedSquares[move.From].Occupant.MoveSquares.Add(Board.IndexedSquares[move.To]);
                //TODO promotions
                
            }
        }

        private static Move CreateMove(IPiece piece, ISquare targetSquare)
        {
            //TODO
            var capturedPiece = (targetSquare.Occupant == null) ? null : targetSquare.Occupant.PieceStruct;
            return new Move(piece.Location.Coords, targetSquare.Coords, piece.PieceStruct, capturedPiece);
        }

        public PlayerType WhitePlayer
        {
            get { return whitePlayer; }

            set
            {
                whitePlayer = value;
                RaisePropertyChanged(() => WhitePlayer);
            }
        }

        public PlayerType BlackPlayer
        {
            get { return blackPlayer; }

            set
            {
                blackPlayer = value;
                RaisePropertyChanged(() => BlackPlayer);
            }
        }
        public Game()
        {
            WhitePlayer = PlayerType.Human;
            BlackPlayer = PlayerType.Human;

            Restart();
        }

        public void Restart()
        {
            Board = new Board();
            InternalBoard = new InternalBoard();
            Board.Initialize();
            CurrentPlayer = Player.White;
            ReSync();
            IsActive = true;
            CheckAI();

        }

        private void SwapPlayer()
        {
            CurrentPlayer = OTHER_PLAYER(CurrentPlayer);
            Board.SelectedPiece = null;
            CheckAI();
        }

        private void CheckAI()
        {
            //TODO
        }


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
    }
}
