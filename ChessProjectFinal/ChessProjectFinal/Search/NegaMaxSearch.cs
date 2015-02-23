using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Search
{
    public class NegaMaxSearch :ISearch 
    {
        internal struct ValueDepth
        {
            public readonly int Value;
            public readonly int Depth;
            public override bool Equals(object obj)
            {
                if (!(obj is ValueDepth)) return false;
                var that = (ValueDepth) obj;
                return (Value == that.Value && Depth==that.Depth);
            }
            public override int GetHashCode()
            {
               return  Value* Depth;
            }

            public ValueDepth(int depth, int value)
            {
                Value = value;
                Depth = depth;
            }
        }
        private const int INFINITY = 100000;
        private readonly Dictionary<IState,ValueDepth> table= new Dictionary<IState, ValueDepth>();

        private void addToTable(IState state, int depth, int value)
        {
            ValueDepth tableValueDepth;
            var exists = table.TryGetValue(state, out tableValueDepth);
            if (!exists || tableValueDepth.Depth<depth)
                table.Add(state,new ValueDepth(depth,value));

        }

        private readonly IHeuristic heuristic;
        private readonly IOrdering ordering; 

        public NegaMaxSearch(IHeuristic heuristic,IOrdering ordering)
        {
            this.heuristic = heuristic;
            this.ordering = ordering;
        }

        private IAction iterativeDepeningSearch(Node node, int depth, int side)
        {
            IAction bestAction=new NoAction();
            table.Clear();
            for (var i = 0; i < depth; i++)
            {
                var value=search(node, depth, -INFINITY, INFINITY, side,out bestAction);
            }
            return bestAction;


        }
        private int search(Node node, int depth, int alpha, int beta, int side,out IAction bestAction)
        {
             bestAction = new NoAction();
            Node bestNode = null;
            if (depth == 0 || node.State.IsTerminal())
            {
                
                ValueDepth tableEntry;
                if (!table.TryGetValue(node.State,out tableEntry))
                {
                    var value=heuristic.GetValue(node.State);
                    table.Add(node.State,new ValueDepth(depth,value));
                    return value*side;
                }
                return tableEntry.Value;
            }
            var bestValue = -INFINITY;
            var actions = node.State.GetActions();
            var children = actions.Select(action => new Node(node.State.GetActionResult(action),action,node)).ToList();
            sort(children,side);
            foreach (var child  in children)
            {
                IAction possibleAction;
                var possibleValue = -search(child, depth - 1, -beta, -alpha, -side, out possibleAction);
                if (possibleValue > bestValue)
                {
                    bestValue = possibleValue;
                    bestNode = child;
                }
                if (alpha > beta)
                    break;
            }

            Debug.Assert(bestNode != null, "bestNode != null");
            addToTable(bestNode.State,depth,bestValue);
            bestAction = bestNode.Action;
            return bestValue;
           
        }

        private int compareNodes(Node n1, Node n2)
        {
            if (n1 == null && n2 == null)
                return 0;
            if (n1 == null)
                return -1;
            if (n2 == null)
                return 1;
           ValueDepth valueDepth1;
           ValueDepth valueDepth2;
            var exists1 = table.TryGetValue(n1.State, out valueDepth1);
            var exists2 = table.TryGetValue(n2.State, out valueDepth2);
           if (exists1 && exists2)
               return Comparer<int>.Default.Compare(valueDepth1.Value, valueDepth2.Value);
            if (exists1)
                return 1;
            if (exists2)
                return -1;
            var move1 = (Move) n1.Action;
            var move2 = (Move) n2.Action;
            if (move1.CapturedPiece != null && move2.CapturedPiece==null)
                return 1;
            if (move1.CapturedPiece == null && move2.CapturedPiece == null)
                return -1;
            return 0;



        }

        private void sort(List<Node> children,int side)
        {
            children.Sort(Comparer<Node>.Create(compareNodes));
        }


        public IAction SearchByDepth(Node node, int plys, int side)
        {
            var depth = 2*plys - 1;
            IAction action;
            search(node, depth, -INFINITY, INFINITY, side, out action);
            return action;

        }

        public IAction SearchByTime(Node node, int time, int side)
        {
            throw new NotImplementedException();
        }
    }
}
