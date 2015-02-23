using System;

namespace ChessProjectFinal.Search
{
    public interface ISearch
    {
        IAction SearchByDepth(Node node,int depth, int side);
        IAction SearchByTime(Node node, int time,  int side);

    }
}
