using System;

namespace ChessProjectFinal.Entities
{
    public struct TableEntry
    {
        public UInt64 Zobrist;
        public int Depth;
        public int Flag;
        public int Eval;
        public int Ancient;
        public Move Move;

        public TableEntry(UInt64 zobrist,int depth,int flag,int eval,int ancient,Move move)
        {
            Zobrist = zobrist;
            Depth = depth;
            Flag = flag;
            Eval = eval;
            Ancient = ancient;
            Move = move;
        }

    }
}
