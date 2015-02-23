using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChessProjectFinal.Model
{
    public class BoardState
    {
        public static BoardState DefaultBoard()
        {
            var pieceBoard = new Piece[8, 8];
            for (var i = 0; i < 8; i++)
                foreach (Player player in Enum.GetValues((typeof(Player))))
                    pieceBoard[Game.PAWN_ROW(player), i] = Piece.PIECES[player][PieceType.Pawn];

            foreach (Player color in Enum.GetValues(typeof(Player)))
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

            
            return new BoardState(pieceBoard, false, new Point(0,0), new CastlingRights(true, true),new CastlingRights(true,true), Player.White);
            
          
        }
        public static BoardState DoMove(BoardState boardState,IMove move)
        {
            var newState=new BoardState(boardState);
            var x = (int)move.From.X;
            var y = (int)move.From.Y;
            var x2 = (int)move.To.X;
            var y2 = (int)move.To.Y;
            newState.PieceBoard[x, y] = null;
            newState.PieceBoard[x2, y2] = move.Piece;
            if (move.Promotion != null)
            {
                newState.PieceBoard[x2, y2] = move.Promotion;
            }
            if (move.Piece.PieceType == PieceType.Rook || move.Piece.PieceType == PieceType.King)
            {
                newState.CastleKingSide[move.Piece.Player] = false;
                newState.CastleQueenSide[move.Piece.Player] = false;
            }
            if (move.Piece.PieceType == PieceType.Pawn && Math.Abs(x - x2) == 2)
            {
                newState.EnPassant = true;
                newState.EnPassantSquare = move.To;
            }
            else
                newState.EnPassant = false;
            if (move.IsEnPassant)
                newState.PieceBoard[x, y2] = null;
            var baseRow = move.Piece.Player == Player.White ? 0 : 7;
            if (move.IsKingSideCastle)
            {
                newState.PieceBoard[baseRow, 7] = null;
                newState.PieceBoard[baseRow, 5] = Piece.PIECES[move.Piece.Player][PieceType.Rook];
            }
            if (move.IsQueenSideCastle)
            {
                newState.PieceBoard[baseRow, 0] = null;
                newState.PieceBoard[baseRow, 3] = Piece.PIECES[move.Piece.Player][PieceType.Rook];
            }
            newState.CurrentPlayer = Game.OTHER_PLAYER(boardState.CurrentPlayer);
            return newState;
        }
        public static IReadOnlyList<Move> GetValidMoves(BoardState boardState,Player player)
        {
            var moves = getMoves(boardState, player);
            var validMoves = new List<Move>(moves);
            foreach (var move in moves)
            {
                if (move.IsKingSideCastle)
                    for (var i = 5; i < 7; i++)
                        if (isAttacked(boardState,Game.OTHER_PLAYER(move.Piece.Player), Game.BASE_ROW(move.Piece.Player), i) || boardState.PieceBoard[Game.BASE_ROW(move.Piece.Player), i] != null)
                               validMoves.Remove(move);
                    
                if (move.IsQueenSideCastle)
                    for (var i = 1; i < 4; i++)
                        if (isAttacked(boardState,Game.OTHER_PLAYER(move.Piece.Player), Game.BASE_ROW(move.Piece.Player), i) || boardState.PieceBoard[Game.BASE_ROW(move.Piece.Player), i] != null)
                              validMoves.Remove(move);
                if (isCheck(DoMove(boardState, move), player))
                    validMoves.Remove(move);


            }
            return validMoves;


        }

        public static bool IsCheckMate(BoardState boardState,Player player )
        {
            return GetValidMoves(boardState, player).Count == 0;
        }

        public static bool IsStaleMate(BoardState boardState, Player player)
        {
            return GetValidMoves(boardState, player).Count == 0 && isCheck(boardState, player);
        }

        private static List<Move> getMoves(BoardState boardState,Player player)
        {
            
            var moveList = new List<Move>();
            for (var i = 0; i < 8; i++)
                for (var j = 0; j < 8; j++)
                {
                    var from = new Point(i, j);
                    var piece = boardState.PieceBoard[i, j];
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
                                        while (x >= 0 && x <= 7 && y >= 0 && y <= 7 && boardState.PieceBoard[x, y] == null)
                                        {
                                            moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                            x += d1;
                                            y += d2;
                                        }
                                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7 && (boardState.PieceBoard[x, y] == null || boardState.PieceBoard[x, y].Player != piece.Player))
                                            moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                    }

                                break;
                            case PieceType.Rook:
                                foreach (var d1 in new[] { -1, 1 })
                                {
                                    var x = i + d1;
                                    while (x >= 0 && x <= 7 && boardState.PieceBoard[x, j] == null)
                                    {
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                        x += d1;
                                    }
                                    if (x >= 0 && x <= 7 && (boardState.PieceBoard[x, j] == null || boardState.PieceBoard[x, j].Player != piece.Player))
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                    x = j + d1;
                                    while (x >= 0 && x <= 7 && boardState.PieceBoard[i, x] == null)
                                    {
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(i, x) });
                                        x += d1;
                                    }
                                    if (x >= 0 && x <= 7 && (boardState.PieceBoard[i, x] == null || boardState.PieceBoard[i, x].Player != piece.Player))
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(i, x) });
                                }
                                break;
                            case PieceType.Pawn:
                                var direction = piece.Player == Player.White ? 1 : -1;
                                var pawnRow = piece.Player == Player.White ? 1 : 6;

                                if (boardState.EnPassant && (boardState.EnPassantSquare == new Point(i, j + 1) || boardState.EnPassantSquare == new Point(i, j - 1)))
                                    moveList.Add(new Move(from, new Point(i + direction, boardState.EnPassantSquare.Y), piece, boardState.PieceBoard[i, (int)boardState.EnPassantSquare.Y]) { IsEnPassant = true });

                                if (boardState.PieceBoard[i + direction, j] == null)
                                {
                                    moveList.Add(new Move(from, new Point(i + direction, j), piece, null));
                                    if (i == pawnRow && boardState.PieceBoard[i + direction * 2, j] == null)
                                        moveList.Add(new Move(from, new Point(i + direction * 2, j), piece, null));
                                }

                                if (j > 0 && boardState.PieceBoard[i + direction, j - 1] != null && boardState.PieceBoard[i + direction, j - 1].Player != piece.Player)
                                    moveList.Add(new Move(from, new Point(i + direction, j - 1), piece, boardState.PieceBoard[i + direction, j - 1]));
                                if (j < 7 && boardState.PieceBoard[i + direction, j + 1] != null && boardState.PieceBoard[i + direction, j + 1].Player != piece.Player)
                                    moveList.Add(new Move(from, new Point(i + direction, j + 1), piece, boardState.PieceBoard[i + direction, j + 1]));
                                break;
                            case PieceType.Knight:
                                var dir1 = new[] { 1, -1, 1, -1, 2, -2, 2, -2 };
                                var dir2 = new[] { 2, 2, -2, -2, 1, 1, -1, -1 };
                                for (int k = 0; k < 8; k++)
                                {
                                    var a = i + dir1[k];
                                    var b = j + dir2[k];
                                    if (a >= 0 && a <= 7 && b >= 0 && b <= 7 && (boardState.PieceBoard[a, b] == null || boardState.PieceBoard[a, b].Player != piece.Player))
                                        moveList.Add(new Move(from, new Point(a, b), piece, boardState.PieceBoard[a, b]));
                                }
                                break;
                            case PieceType.King:
                                var dirs1 = new[] { 1, 1, 1, 0, 0, -1, -1, -1 };
                                var dirs2 = new[] { 1, 0, -1, 1, -1, 1, 0, -1 };
                                for (var k = 0; k < 8; k++)
                                {
                                    var a = i + dirs1[k];
                                    var b = j + dirs2[k];
                                    if (a >= 0 && a <= 7 && b >= 0 && b <= 7 && (boardState.PieceBoard[a, b] == null || boardState.PieceBoard[a, b].Player != piece.Player))
                                        moveList.Add(new Move(from, new Point(a, b), piece, boardState.PieceBoard[a, b]));

                                }
                                if (boardState.CastleKingSide[piece.Player])
                                    moveList.Add(new Move(from, new Point(Game.BASE_ROW(piece.Player), 6), piece, null) { IsKingSideCastle = true });
                                if (boardState.CastleQueenSide[piece.Player])
                                    moveList.Add(new Move(from, new Point(Game.BASE_ROW(piece.Player), 2), piece, null) { IsQueenSideCastle = true });


                                break;
                            case PieceType.Queen:
                                foreach (var d1 in new[] { -1, 1 })
                                    foreach (var d2 in new[] { -1, 1 })
                                    {
                                        var x = i + d1;
                                        var y = j + d2;
                                        while (x >= 0 && x <= 7 && y >= 0 && y <= 7 && boardState.PieceBoard[x, y] == null)
                                        {
                                            moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                            x += d1;
                                            y += d2;
                                        }
                                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7 && (boardState.PieceBoard[x, y] == null || boardState.PieceBoard[x, y].Player != piece.Player))
                                            moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, y], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, y) });
                                    }
                                foreach (var d1 in new[] { -1, 1 })
                                {
                                    var x = i + d1;
                                    while (x >= 0 && x <= 7 && boardState.PieceBoard[x, j] == null)
                                    {
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                        x += d1;
                                    }
                                    if (x >= 0 && x <= 7 && (boardState.PieceBoard[x, j] == null || boardState.PieceBoard[x, j].Player != piece.Player))
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[x, j], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(x, j) });
                                    x = j + d1;
                                    while (x >= 0 && x <= 7 && boardState.PieceBoard[i, x] == null)
                                    {
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(i, x) });
                                        x += d1;
                                    }
                                    if (x >= 0 && x <= 7 && (boardState.PieceBoard[i, x] == null || boardState.PieceBoard[i, x].Player != piece.Player))
                                        moveList.Add(new Move { CapturedPiece = boardState.PieceBoard[i, x], From = from, IsEnPassant = false, IsKingSideCastle = false, IsQueenSideCastle = false, Piece = piece, Promotion = null, To = new Point(i, x) });
                                }
                                break;


                        }

                    }
                }
            return moveList;
        } 
        private static Boolean isAttacked(BoardState boardState,Player player, int row, int column)
        {
            var moves = getMoves(boardState,player);
            return moves.Any(move => move.To == new Point(row, column) && move.CapturedPiece == boardState.PieceBoard[row, column]);
        }
        private static bool isCheck(BoardState boardState,Player player)
        {
            var moves=getMoves(boardState,Game.OTHER_PLAYER(player));
            return moves.Any(move => move.CapturedPiece != null && move.CapturedPiece.PieceType == PieceType.King);
        }
     
        
        public Piece[,] PieceBoard;
        public bool EnPassant;
        public Point EnPassantSquare;
        public CastlingRights CastleQueenSide = new CastlingRights(false,false);
        public CastlingRights CastleKingSide = new CastlingRights(false,false);
        public Player CurrentPlayer;


        public override bool Equals(object obj)
        {
            if (!(obj is BoardState))
                return false;
            var other = (BoardState) obj;
            for (var i=0; i<8; i++)
                for (var j=0; j<8; j++)
                    if (PieceBoard[i, j] != other.PieceBoard[i, j])
                        return false;
            return CastleKingSide.Equals(other.CastleKingSide) && CastleQueenSide.Equals(CastleQueenSide) &&
                   CurrentPlayer == other.CurrentPlayer;
        }

        public BoardState(Piece[,] pieceBoard,bool enPassant,Point enPassantSquare,CastlingRights castleQueenSide, CastlingRights castleKingSide,Player player)
        {
            CurrentPlayer = player;
            PieceBoard = (Piece[,])pieceBoard.Clone();
            EnPassant = enPassant;
            EnPassantSquare = enPassantSquare;
            CastleKingSide = castleKingSide;
            CastleQueenSide = castleQueenSide;
        }
        public BoardState(BoardState that) : this(that.PieceBoard, that.EnPassant, that.EnPassantSquare, that.CastleQueenSide,that.CastleKingSide, that.CurrentPlayer)
        {
           
        }
       




    }
}
