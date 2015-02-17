using System;
using System.Collections.Generic;
using ChessProjectFinal.Model;
using ChessProjectFinal.Search;

namespace ChessProjectFinal.ChessSearch
{
    class AIState : BoardState, IState
    {
        public AIState(BoardState that) : base(that)
        {
        }

        public IReadOnlyList<IAction> GetActions()
        {
           return new GameHistory(this).GetValidMoves(CurrentPlayer);
        }

        public IState GetActionResult(IAction action)
        {
            var board = new GameHistory(this);
            var move = (Move) action;
            board.DoMove(move);
            return new AIState(board.GetState());
        }
    }
}
