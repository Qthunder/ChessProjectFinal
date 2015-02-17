﻿using System;

namespace ChessProjectFinal.Search
{
    public class NegaMaxSearch :ISearch 
    {
        private readonly IHeuristic heuristic;
        private readonly IOrdering ordering; 
        public NegaMaxSearch(IHeuristic heuristic,IOrdering ordering)
        {
            this.heuristic = heuristic;
            this.ordering = ordering;
        }
       

        private Tuple<IAction, int> search(Node node, int depth, int alpha, int beta, int side)
        {
             //TODO might have to check explicitly if node is terminal (atm i'm assuming the case will be resolved naturally)
            if (depth == 0)
                return new Tuple<IAction, int>(node.Action,side*heuristic.GetValue(node.State));
            var bestValue = int.MinValue;
            IAction bestAction = null;
            var  actions = node.State.GetActions();
            actions = ordering.Sort(actions);
            foreach (var action in actions)
            {
                var child = node.State.GetActionResult(action);
                var possibleResult= search(new Node(child,action), depth - 1, -beta, -alpha, -side);
                if (bestValue < possibleResult.Item2)
                {
                    bestValue = possibleResult.Item2;
                    bestAction = action;
                }
                alpha = Math.Max(alpha, possibleResult.Item2);
                if (alpha >= beta)
                    break;
            }
            return Tuple.Create(bestAction, bestValue);

        }


        public Tuple<IAction, int> SearchByDepth(Node node, int depth, int side)
        {
            return this.search(node, 2*depth-1, int.MinValue, int.MaxValue, side);
        }

        public Tuple<IAction, int> SearchByTime(Node node, int time, int side)
        {
            throw new NotImplementedException();
        }
    }
}