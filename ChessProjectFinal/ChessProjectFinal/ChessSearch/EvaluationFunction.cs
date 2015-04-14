using System;
using ChessProjectFinal.Entities;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ChessSearch
{
    public class EvaluationFunction 
    {
        public int GetValue(BoardState boardState)
        {
            var sideToPlay = boardState.CurrentPlayer == Player.WHITE ? 1 : -1;
            if (BoardState.IsCheckMate(boardState, boardState.CurrentPlayer))
                return 100000*sideToPlay*-1;
            if (BoardState.IsStaleMate(boardState, boardState.CurrentPlayer))
                return 0;
            var material = 0;
            for (var i=0; i<8; i++)
                for (var j = 0; j < 8; j++)
                {
                    if (boardState.PieceBoard[i, j] == null) continue;
                    var side = boardState.PieceBoard[i, j].Player == Player.WHITE ? 1 : -1;
                    switch (boardState.PieceBoard[i, j].PieceType)
                    {
                        case PieceType.ROOK:
                            material += side*500;
                            break;
                        case PieceType.KNIGHT:
                            material += side*300;
                            break;
                        case PieceType.BISHOP:
                            material +=side* 300;
                            break;
                        case PieceType.QUEEN:
                            material += side*900;
                            break;
                        case PieceType.PAWN:
                            material +=side* 100;
                            break;
                    }
                }
            
            var mobility =10 *(BoardState.GetValidMoves(boardState,Player.WHITE).Count - BoardState.GetValidMoves(boardState,Player.BLACK).Count);
            var doublePawns = 0;
            foreach (Player player in Enum.GetValues(typeof (Player)))
            { var side = player == Player.WHITE ? -1 : 1;
                for (var j = 0; j < 8; j++)
                {   var pawnCount=0;
                    for (var i = 0; i < 7; i++)
                        if (boardState.PieceBoard[i, j] == Piece.PIECES[player][PieceType.PAWN])
                            pawnCount++;
                    if (pawnCount >= 2)
                        doublePawns += (pawnCount - 1)*side*50;

                }
            }

            return material+mobility+doublePawns;
        }
    }
}
