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
            return GetValidMoves(this, CurrentPlayer); 
        }

        public IState GetActionResult(IAction action)
        {
            var move = (Move) action;
            return new AIState(DoMove(this, move));
        }

        public bool IsTerminal()
        {
            return IsCheckMate(this,CurrentPlayer) || IsStaleMate(this,CurrentPlayer);
        }
    }
}
