using System.Collections.Generic;

namespace ChessProjectFinal.Model
{
    public interface IInternalBoard
    {
        void MakeMove(IMove move);
        List<Move> GetMoves(Player player);
        PieceStruct[,] GetState();
    }
}
