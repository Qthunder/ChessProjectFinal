using System;

namespace ChessProjectFinal.Search
{
    public interface ISearch
    {
        Tuple<IAction,int> SearchByDepth(Node node,int depth, int side);
        Tuple<IAction, int> SearchByTime(Node node, int time,  int side);

    }
}
