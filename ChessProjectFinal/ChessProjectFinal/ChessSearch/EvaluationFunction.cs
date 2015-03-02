using System;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ChessSearch
{
    public class EvaluationFunction 
    {
        public int GetValue(BoardState boardState)
        {
            var sideToPlay = boardState.CurrentPlayer == Player.White ? 1 : -1;
            if (BoardState.IsCheckMate(boardState, boardState.CurrentPlayer))
                return 1000*sideToPlay*-1;
            if (BoardState.IsStaleMate(boardState, boardState.CurrentPlayer))
                return 0;
            var material = 0;
            for (var i=0; i<8; i++)
                for (var j = 0; j < 8; j++)
                {
                    if (boardState.PieceBoard[i, j] == null) continue;
                    var side = boardState.PieceBoard[i, j].Player == Player.White ? 1 : -1;
                    switch (boardState.PieceBoard[i, j].PieceType)
                    {
                        case PieceType.Rook:
                            material += side*500;
                            break;
                        case PieceType.Knight:
                            material += side*300;
                            break;
                        case PieceType.Bishop:
                            material +=side* 300;
                            break;
                        case PieceType.Queen:
                            material += side*900;
                            break;
                        case PieceType.Pawn:
                            material +=side* 100;
                            break;
                    }
                }
            
            var mobility =10 *(BoardState.GetValidMoves(boardState,Player.White).Count - BoardState.GetValidMoves(boardState,Player.Black).Count);
            var doublePawns = 0;
            foreach (Player player in Enum.GetValues(typeof (Player)))
            { var side = player == Player.White ? -1 : 1;
                for (var j = 0; j < 8; j++)
                {   var pawnCount=0;
                    for (var i = 0; i < 7; i++)
                        if (boardState.PieceBoard[i, j] == Piece.PIECES[player][PieceType.Pawn])
                            pawnCount++;
                    if (pawnCount >= 2)
                        doublePawns += (pawnCount - 1)*side*50;

                }
            }

            return material+mobility+doublePawns;
        }
    }
}
