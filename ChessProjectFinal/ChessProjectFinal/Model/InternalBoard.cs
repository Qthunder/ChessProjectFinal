/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChessProjectFinal.Model
{

    public class GameHistory :IInternalBoard
    {

        #region STATE
        private readonly Stack<BoardState> history=new Stack<BoardState>();
        private Piece[,] pieceBoard=new Piece[8,8];
        private bool enPassant;
        private Point enPassantSquare ;
        private Dictionary<Player, Boolean> castleQueenSide=new Dictionary<Player, bool>();
        private Dictionary<Player, Boolean> castleKingSide=new Dictionary<Player, bool>();
        private Player currentPlayer = Player.White; 
        #endregion
        #region CONSTRUCTORS
        public GameHistory()
        {
            restart();
        }

        public GameHistory(BoardState boardState)
        {
            RestoreState(boardState);
            this.history.Clear();
        }
#endregion
        #region PUBLIC METHODS
        public void DoMove(IMove move)
        {
            currentPlayer = Game.OTHER_PLAYER(currentPlayer);
            this.doMove(move);

        }

        private void doMove(IMove move)
        {
            
            this.history.Push(this.GetState());
            var x = (int)move.From.X;
            var y = (int)move.From.Y;
            var x2 = (int)move.To.X;
            var y2 = (int)move.To.Y;
            pieceBoard[x, y] = null;
            pieceBoard[x2, y2] = move.Piece;
            if (move.Promotion != null)
            {
                pieceBoard[x2, y2] = move.Promotion;
            }
            if (move.Piece.PieceType == PieceType.Rook || move.Piece.PieceType == PieceType.King)
            {
                castleKingSide[move.Piece.Player] = false;
                castleQueenSide[move.Piece.Player] = false;
            }
            if (move.Piece.PieceType == PieceType.Pawn && Math.Abs(x - x2) == 2)
            {
                enPassant = true;
                enPassantSquare = move.To;
            }
            else
                enPassant = false;
            if (move.IsEnPassant)
                pieceBoard[x, y2] = null;
            var baseRow = move.Piece.Player == Player.White ? 0 : 7;
            if (move.IsKingSideCastle)
            {
                pieceBoard[baseRow, 7] = null;
                pieceBoard[baseRow, 5] = Piece.PIECES[move.Piece.Player][PieceType.Rook];
            }
            if (move.IsQueenSideCastle)
            {
                pieceBoard[baseRow, 0] = null;
                pieceBoard[baseRow, 3] = Piece.PIECES[move.Piece.Player][PieceType.Rook];
            }
        }


        public void UndoMove()
        {

           RestoreState(history.Pop());
        } 
        public BoardState GetState()
        {
            return new BoardState(pieceBoard,enPassant,enPassantSquare,castleQueenSide,castleKingSide,currentPlayer);
        }
        public IReadOnlyList<Move> GetValidMoves(Player player)
        {
            var moves = this.getMoves(player);
            var validmoves = moves.GetRange(0, moves.Count);
            foreach (var move in moves)
            {
                if (move.IsKingSideCastle  )
                {
                    var castle = true;
                    for (var i = 5; i < 7; i++)
                        if (this.isAttacked(Game.OTHER_PLAYER(move.Piece.Player), Game.BASE_ROW(move.Piece.Player), i) || pieceBoard[Game.BASE_ROW(move.Piece.Player), i] != null)
                            castle = false;
                    if (!castle)
                        validmoves.Remove(move);
                }
                if (move.IsQueenSideCastle )
                {   //check for check
                    var castle = true;
                    for (var i = 1; i < 4; i++)
                        if (this.isAttacked(Game.OTHER_PLAYER(move.Piece.Player), Game.BASE_ROW(move.Piece.Player), i) || pieceBoard[Game.BASE_ROW(move.Piece.Player), i] != null)
                            castle = false;
                    if (!castle)
                        validmoves.Remove(move);
                }
                doMove(move);
                if (isCheck(player))
                    validmoves.Remove(move);
               UndoMove();

            }
            return validmoves;
        }

        public void RestoreState(BoardState boardState)
        {
            pieceBoard =(Piece[,]) boardState.PieceBoard.Clone();
            enPassant = boardState.EnPassant;
            enPassantSquare = boardState.EnPassantSquare;
            castleKingSide = new Dictionary<Player, bool>
            {
                {Player.White, boardState.CastleKingSide[Player.White]},
                {Player.Black, boardState.CastleKingSide[Player.Black]}
            };
            castleQueenSide = new Dictionary<Player, bool>
            {
                {Player.White, boardState.CastleQueenSide[Player.White]},
                {Player.Black, boardState.CastleQueenSide[Player.Black]}
            };

        }
        #endregion
        #region OTHER METHODS
        private void restart()
        {
            currentPlayer = Player.White;
            for (var i = 0; i < 8; i++)
                foreach (Player player in Enum.GetValues((typeof(Player))))
                    pieceBoard[Game.PAWN_ROW(player), i] = Piece.PIECES[player][PieceType.Pawn];
           
            foreach (Player color in Enum.GetValues(typeof (Player)))
            {
                var row = color == Player.White ? 0 : 7;
                pieceBoard[row, 0] = Piece.PIECES[color][PieceType.Rook];
                pieceBoard[row, 7] = Piece.PIECES[color][PieceType.Rook];
                pieceBoard[row, 1] = Piece.PIECES[color][PieceType.Knight];
                pieceBoard[row, 6] = Piece.PIECES[color][PieceType.Knight];
                pieceBoard[row, 2] = Piece.PIECES[color][PieceType.Bishop];
                pieceBoard[row, 5] = Piece.PIECES[color][PieceType.Bishop];
                pieceBoard[row, 3] = Piece.PIECES[color][PieceType.Queen];
                pieceBoard[row, 4] = Piece.PIECES[color][PieceType.King];

            }

          castleQueenSide.Add(Player.White,true);
          castleQueenSide.Add(Player.Black, true);
          castleKingSide.Add(Player.White, true);
          castleKingSide.Add(Player.Black, true);
          this.history.Clear();
        }
        private List<Move> getMoves(Player player)
         {
             var moveList = new List<Move>();
             for (var i = 0; i < 8; i++)
                 for (var j = 0; j < 8; j++)
                 {
                     var from = new Point(i, j);
                     var piece = pieceBoard[i, j];
                     if (piece != null && piece.Player == player)
                     {
                         switch (piece.PieceType)
                         {
                             case PieceType.Bishop:
                                 foreach (var d1 in new[] { -1, 1 })
                                     foreach (var d2 in new[] { -1, 1 })
                                     {
                                         var x = i + d1;
                                         var y = j + d2;
                                         while (x >= 0 && x <= 7 && y >= 0 && y <= 7 && pieceBoard[x, y] == null)
                                         {
                                             moveList.Add(new Move { CapturedPiece = pieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                             x += d1;
                                             y += d2;
                                         }
                                         if (x >= 0 && x <= 7 && y >= 0 && y <= 7 && (pieceBoard[x, y] == null || pieceBoard[x, y].Player != piece.Player))
                                             moveList.Add(new Move { CapturedPiece = pieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                     }

                                 break;
                             case PieceType.Rook:
                                 foreach (var d1 in new[] { -1, 1 })
                                 {
                                     var x = i + d1;
                                     while (x >= 0 && x <= 7 && pieceBoard[x, j] == null)
                                     {
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                         x += d1;
                                     }
                                     if (x >= 0 && x <= 7 && (pieceBoard[x, j] == null || pieceBoard[x, j].Player != piece.Player))
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                     x = j + d1;
                                     while (x >= 0 && x <= 7 && pieceBoard[i, x] == null)
                                     {
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(i, x) });
                                         x += d1;
                                     }
                                     if (x >= 0 && x <= 7 && (pieceBoard[i, x] == null || pieceBoard[i, x].Player != piece.Player))
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                 }
                                 break;
                             case PieceType.Pawn:
                                 var direction = piece.Player == Player.White ? 1 : -1;
                                 var pawnRow = piece.Player == Player.White ? 1 : 6;

                                 if (enPassant && (enPassantSquare == new Point(i, j + 1) || enPassantSquare == new Point(i, j - 1)))
                                     moveList.Add(new Move(from, new Point(i + direction, enPassantSquare.Y), piece, pieceBoard[i, (int)this.enPassantSquare.Y]) { IsEnPassant = true });

                                 if (pieceBoard[i + direction, j] == null)
                                 {
                                     moveList.Add(new Move(from, new Point(i + direction, j), piece, null));
                                     if (i == pawnRow && pieceBoard[i + direction * 2, j] == null)
                                         moveList.Add(new Move(from, new Point(i + direction * 2, j), piece, null));
                                 }

                                 if (j > 0 && pieceBoard[i + direction, j - 1] != null && pieceBoard[i + direction, j - 1].Player != piece.Player)
                                     moveList.Add(new Move(from, new Point(i + direction, j - 1), piece, pieceBoard[i + direction, j - 1]));
                                 if (j < 7 && pieceBoard[i + direction, j + 1] != null && pieceBoard[i + direction, j + 1].Player != piece.Player)
                                     moveList.Add(new Move(from, new Point(i + direction, j + 1), piece, pieceBoard[i + direction, j + 1]));
                                 break;
                             case PieceType.Knight:
                                 var dir1 = new[] { 1, -1, 1, -1, 2, -2, 2, -2 };
                                 var dir2 = new[] { 2, 2, -2, -2, 1, 1, -1, -1 };
                                 for (int k = 0; k < 8; k++)
                                 {
                                     var a = i + dir1[k];
                                     var b = j + dir2[k];
                                     if (a >= 0 && a <= 7 && b >= 0 && b <= 7 && (pieceBoard[a, b] == null || pieceBoard[a, b].Player != piece.Player))
                                         moveList.Add(new Move(from, new Point(a, b), piece, pieceBoard[a, b]));
                                 }
                                 break;
                             case PieceType.King:
                                 var dirs1 = new[] { 1, 1, 1, 0, 0, -1, -1, -1 };
                                 var dirs2 = new[] { 1, 0, -1, 1, -1, 1, 0, -1 };
                                 for (var k = 0; k < 8; k++)
                                 {
                                     var a = i + dirs1[k];
                                     var b = j + dirs2[k];
                                     if (a >= 0 && a <= 7 && b >= 0 && b <= 7 && (pieceBoard[a, b] == null || pieceBoard[a, b].Player != piece.Player))
                                         moveList.Add(new Move(from, new Point(a, b), piece, pieceBoard[a, b]));

                                 }
                                 if (castleKingSide[piece.Player])
                                     moveList.Add(new Move(from,new Point(Game.BASE_ROW(piece.Player),6),piece,null){IsKingSideCastle = true});
                                 if (castleQueenSide[piece.Player])
                                     moveList.Add(new Move(from, new Point(Game.BASE_ROW(piece.Player), 2), piece, null) { IsQueenSideCastle = true });


                                 break;
                             case PieceType.Queen:
                                 foreach (var d1 in new[] { -1, 1 })
                                     foreach (var d2 in new[] { -1, 1 })
                                     {
                                         var x = i + d1;
                                         var y = j + d2;
                                         while (x >= 0 && x <= 7 && y >= 0 && y <= 7 && pieceBoard[x, y] == null)
                                         {
                                             moveList.Add(new Move { CapturedPiece = pieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                             x += d1;
                                             y += d2;
                                         }
                                         if (x >= 0 && x <= 7 && y >= 0 && y <= 7 && (pieceBoard[x, y] == null || pieceBoard[x, y].Player != piece.Player))
                                             moveList.Add(new Move { CapturedPiece = pieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                     }
                                 foreach (var d1 in new[] { -1, 1 })
                                 {
                                     var x = i + d1;
                                     while (x >= 0 && x <= 7 && pieceBoard[x, j] == null)
                                     {
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                         x += d1;
                                     }
                                     if (x >= 0 && x <= 7 && (pieceBoard[x, j] == null || pieceBoard[x, j].Player != piece.Player))
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                     x = j + d1;
                                     while (x >= 0 && x <= 7 && pieceBoard[i, x] == null)
                                     {
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(i, x) });
                                         x += d1;
                                     }
                                     if (x >= 0 && x <= 7 && (pieceBoard[i,x] == null || pieceBoard[i, x].Player != piece.Player))
                                         moveList.Add(new Move { CapturedPiece = pieceBoard[i,x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                 }
                                 break;


                         }

                     }
                 }
             return moveList;
         } 
        private bool isAttacked(Player player, int row, int column)
        {
            var moves = this.getMoves(player);
            return moves.Any(move => move.To == new Point(row, column) && move.CapturedPiece == this.pieceBoard[row, column]);
        }
        private bool isCheck(Player color)
        {
            var moves=this.getMoves((Game.OTHER_PLAYER(color)));
            return moves.Any(move => move.CapturedPiece != null && move.CapturedPiece.PieceType == PieceType.King);
        }
        #endregion

    }
}
*/
