using System.Collections.Generic;

namespace ChessProjectFinal.Search
{
    public interface IOrdering
    {
        IList<IAction> Sort(IList<IAction> actions );
    }
}


