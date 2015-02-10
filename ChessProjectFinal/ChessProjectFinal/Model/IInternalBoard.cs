using System.Collections.Generic;

namespace ChessProjectFinal.Model
{
    public interface IInternalBoard
    {
        void DoMove(IMove move);
        void UndoMove();
        List<Move> GetValidMoves(Player player);
        BoardState GetState();
        void RestoreState(BoardState boardState);
    }
}
