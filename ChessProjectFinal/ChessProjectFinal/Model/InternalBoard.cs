using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.XPath;

namespace ChessProjectFinal.Model
{

    public class InternalBoard :IInternalBoard
    {
        private readonly PieceStruct[,] pieceBoard=new PieceStruct[8,8];
        private bool enPassant = false;
        private Point enPassantSquare ;


        public InternalBoard()
        {
            restart();
        }

        
        private void restart()
        {
            for (var i = 0; i < 8; i++)
            {
                pieceBoard[1, i] = new PieceStruct {Color = Player.White, Piece = PieceType.Pawn};
                pieceBoard[6, i] = new PieceStruct {Color = Player.Black, Piece = PieceType.Pawn};
            }
            foreach (Player color in Enum.GetValues(typeof (Player)))
            {
                var row = color == Player.White ? 0 : 7;
                pieceBoard[row, 0] = new PieceStruct {Color = color, Piece = PieceType.Rook};
                pieceBoard[row, 7] = new PieceStruct { Color = color, Piece = PieceType.Rook };
                pieceBoard[row, 1] = new PieceStruct { Color = color, Piece = PieceType.Knight };
                pieceBoard[row, 6] = new PieceStruct { Color = color, Piece = PieceType.Knight };
                pieceBoard[row, 2] = new PieceStruct { Color = color, Piece = PieceType.Bishop };
                pieceBoard[row, 5] = new PieceStruct { Color = color, Piece = PieceType.Bishop };
                pieceBoard[row, 3] = new PieceStruct { Color = color, Piece = PieceType.Queen };
                pieceBoard[row, 4] = new PieceStruct { Color = color, Piece = PieceType.King };

            }
        }

        public void MakeMove(IMove move)
        {
            var x =(int) move.From.X;
            var y = (int) move.From.Y;
            var x2=(int) move.To.X;
            var y2 = (int) move.To.Y;
            pieceBoard[x, y] = null;
            pieceBoard[x2, y2] = move.Piece;
            if (move.Piece.Piece == PieceType.Pawn && Math.Abs(x - x2) == 2)
            {
                enPassant = true;
                enPassantSquare = move.To;
            }
            else
                enPassant = false;
            if (move.IsEnPassant)
                pieceBoard[x, y2] = null;
            var baseRow = move.Piece.Color == Player.White ? 0 : 7;
            if (move.IsKingSideCastle)
            {
                pieceBoard[baseRow, 7] = null;
                pieceBoard[baseRow, 5] = new PieceStruct {Color = move.Piece.Color, Piece = PieceType.Rook,};
            }
            if (move.IsQueenSideCastle)
            {
                pieceBoard[baseRow, 0] = null;
                pieceBoard[baseRow, 3] = new PieceStruct { Color = move.Piece.Color, Piece = PieceType.Rook, };
            }

               
        }

        public List<Move> GetMoves(Player player)
        {
            var moveList = new List<Move> ();
            for (var i =0 ; i<8; i++)
                for (var j = 0; j < 8; j++)
                {
                    var from = new Point(i, j);
                    var piece = pieceBoard[i, j];
                    if (piece != null && piece.Color==player)
                    {
                        switch (piece.Piece)
                        {
                            case PieceType.Bishop:
                                foreach (var d1 in new [] {-1,1})
                                    foreach (var d2 in new[] {-1, 1})
                                    {
                                        var x = i+d1;
                                        var y = j+d2;
                                        while (x >= 0 && x <= 7 && y >= 0 && y <= 7 && pieceBoard[x,y]==null)
                                        {   moveList.Add(new Move{CapturedPiece = pieceBoard[x,y],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,y)});
                                            x += d1;
                                            y += d2;
                                        }
                                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7 && (pieceBoard[x, y] == null || pieceBoard[x, y].Color != piece.Color))
                                            moveList.Add(new Move{CapturedPiece = pieceBoard[x,y],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,y)});
                                    }
    
                            break;
                            case PieceType.Rook:
                                foreach (var d1 in new [] {-1,1})
                                    {
                                        var x = i+d1;
                                        while (x >= 0 && x <= 7  && pieceBoard[x,j]==null)
                                        {   moveList.Add(new Move{CapturedPiece = pieceBoard[x,j],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,j)});
                                            x += d1;
                                        }
                                        if (x>=0 && x<=7 && (pieceBoard[x,j]==null || pieceBoard[x,j].Color!=piece.Color))
                                            moveList.Add(new Move{CapturedPiece = pieceBoard[x,j],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,j)});
                                        x = j + d1;
                                        while (x >= 0 && x <= 7  && pieceBoard[i,x]==null)
                                            {   moveList.Add(new Move{CapturedPiece = pieceBoard[i,x],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(i,x)});
                                                x += d1;
                                            }
                                        if (x >= 0 && x <= 7 && (pieceBoard[x, j] == null || pieceBoard[i, x].Color != piece.Color))
                                           moveList.Add(new Move{CapturedPiece = pieceBoard[x,j],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,j)});
                                    }
                           break;
                           case PieceType.Pawn:
                                var direction = piece.Color == Player.White ? 1 : -1;
                                var pawnRow = piece.Color == Player.White ? 1 : 6;
                               
                                if (enPassant && (enPassantSquare==new Point(i,j+1) || enPassantSquare==new Point(i,j-1)))
                                    moveList.Add(new Move(from,new Point(i+direction,enPassantSquare.Y),piece, pieceBoard[i,(int) this.enPassantSquare.Y] ){IsEnPassant = true});
                                
                                if (pieceBoard[i + direction, j] == null)
                                {
                                    moveList.Add(new Move(from, new Point(i + direction, j), piece, null));
                                    if (i==pawnRow && pieceBoard[i+direction*2,j]==null)    
                                        moveList.Add(new Move(from,new Point(i+direction*2,j),piece,null ));
                                }

                                if (j>0 && pieceBoard[i+direction,j-1]!=null && pieceBoard[i+direction,j-1].Color!=piece.Color)
                                    moveList.Add(new Move(from,new Point(i+direction,j-1), piece,pieceBoard[i+direction,j-1]));
                                if (j<7 && pieceBoard[i+direction,j+1]!=null && pieceBoard[i+direction,j+1].Color!=piece.Color)
                                    moveList.Add(new Move(from,new Point(i+direction,j+1), piece,pieceBoard[i+direction,j+1]));
                                break;
                            case PieceType.Knight:
                                var dir1 = new[] {1, -1, 1, -1,2,-2,2,-2};
                                var dir2 = new[] {2, 2, -2, -2, 1, 1, -1, -1};
                                for (int k = 0; k < 8; k++)
                                {
                                    var a = i + dir1[k];
                                    var b = j + dir2[k];
                                    if (a>=0 && a<=7 && b>=0 && b<=7 && (pieceBoard[a,b]==null || pieceBoard[a,b].Color!=piece.Color))
                                        moveList.Add(new Move(from,new Point(a,b),piece,pieceBoard[a,b] ));
                                }
                                break;
                            case PieceType.King :
                                var dirs1 = new[] {1, 1, 1, 0, 0, -1, -1, -1};
                                var dirs2 = new[] {1, 0, -1, 1, -1, 1, 0, -1};
                                for (var k = 0; k < 8; k++)
                                {
                                    var a = i + dirs1[k];
                                    var b = j + dirs2[k];
                                    if (a>=0 && a<=7 && b>=0 && b<=7 &&( pieceBoard[a,b]==null || pieceBoard[a,b].Color!=piece.Color))
                                        moveList.Add(new Move(from,new Point(a,b),piece,pieceBoard[a,b] ));
   
                                }
                            break;
                            case PieceType.Queen:
                                 foreach (var d1 in new [] {-1,1})
                                    foreach (var d2 in new[] {-1, 1})
                                    {
                                        var x = i+d1;
                                        var y = j+d2;
                                        while (x >= 0 && x <= 7 && y >= 0 && y <= 7 && pieceBoard[x,y]==null)
                                        {   moveList.Add(new Move{CapturedPiece = pieceBoard[x,y],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,y)});
                                            x += d1;
                                            y += d2;
                                        }
                                        if (x >= 0 && x <= 7 && y >= 0 && y <= 7 && (pieceBoard[x, y] == null || pieceBoard[x, y].Color != piece.Color))
                                            moveList.Add(new Move{CapturedPiece = pieceBoard[x,y],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,y)});
                                    }
                            foreach (var d1 in new [] {-1,1})
                                    {
                                        var x = i+d1;
                                        while (x >= 0 && x <= 7  && pieceBoard[x,j]==null)
                                        {   moveList.Add(new Move{CapturedPiece = pieceBoard[x,j],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,j)});
                                            x += d1;
                                        }
                                        if (x >= 0 && x <= 7 &&( pieceBoard[x, j] == null || pieceBoard[x, j].Color != piece.Color))
                                            moveList.Add(new Move{CapturedPiece = pieceBoard[x,j],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,j)});
                                        x = j + d1;
                                        while (x >= 0 && x <= 7  && pieceBoard[i,x]==null)
                                            {   moveList.Add(new Move{CapturedPiece = pieceBoard[i,x],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(i,x)});
                                                x += d1;
                                            }
                                        if (x >= 0 && x <= 7 &&( pieceBoard[x, j] == null || pieceBoard[i, x].Color != piece.Color))
                                           moveList.Add(new Move{CapturedPiece = pieceBoard[x,j],From=from,IsEnPassant = false,IsKingSideCastle = false,IsQueenSideCastle = false,Piece=piece,Promotion = null,To=new Point(x,j)});
                                    }
                          break;


                        }
                        
                    }
                }
            return moveList;
        }

        public PieceStruct[,] GetState()
        {
            return pieceBoard;
        }
    }
}
