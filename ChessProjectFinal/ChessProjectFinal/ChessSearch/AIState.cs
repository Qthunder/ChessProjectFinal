using System;
using System.Collections.Generic;
using System.Windows;
using ChessProjectFinal.Model;
using ChessProjectFinal.Search;

namespace ChessProjectFinal.ChessSearch
{
    class AIState : BoardState, IState
    {
        public AIState(Piece[,] pieceBoard, bool enPassant, Point enPassantSquare, Dictionary<Player, bool> castleQueenSide, Dictionary<Player, bool> castleKingSide) : base(pieceBoard, enPassant, enPassantSquare, castleQueenSide, castleKingSide)
        {
        }
        public IList<IAction> GetActions()
        {
            throw new System.NotImplementedException();
        }

        public IState GetActionResult(IAction action)
        {
            throw new System.NotImplementedException();
        }
    }
}
