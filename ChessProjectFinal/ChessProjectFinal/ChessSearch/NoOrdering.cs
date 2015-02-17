using System.Collections.Generic;
using ChessProjectFinal.Search;

namespace ChessProjectFinal.ChessSearch
{
    public class NoOrdering :IOrdering
    {
        public IReadOnlyList<IAction> Sort(IReadOnlyList<IAction> actions)
        {
            return actions;
        }
    }
}
