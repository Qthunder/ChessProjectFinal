using System.Collections.Generic;

namespace ChessProjectFinal.Search
{
    public interface IOrdering
    {
        IReadOnlyList<IAction> Sort(IReadOnlyList<IAction> actions);
    }
}


