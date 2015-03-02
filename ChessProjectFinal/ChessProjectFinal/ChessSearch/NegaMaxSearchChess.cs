using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ChessSearch
{
    public class NegaMaxSearchChess
    {


        private static Player player(int side)
        {
            return (side == 1) ? Player.White : Player.Black;
        }

        private const int INFINITY = 10000;
        private readonly EvaluationFunction evaluation;


        public NegaMaxSearchChess(EvaluationFunction evaluation)
        {
            this.evaluation = evaluation;

        }


        public Task<Move> Search(Player player, BoardState boardstate, int depth, int time)
        {

            var timeout = TimeSpan.FromSeconds(time);
            var cts = new CancellationTokenSource(timeout);
            var ct = cts.Token;
            return iterativeDeapeningSearch(player, boardstate, depth, ct);

        }

        private Task<Move> iterativeDeapeningSearch(Player player, BoardState boardState, int depth, CancellationToken ct)
        {   
            var side = player == Player.White ? 1 : -1;
            var searchTask = Task.Factory.StartNew(() =>
            {
                var bestPath = ImmutableStack<Move>.Empty;
                for (var currentDepth = 0; currentDepth < depth; currentDepth++)
                {   
                    ImmutableStack<Move> newPath;
                    searchTillDepth(boardState, currentDepth, -INFINITY*side, INFINITY*side, side, bestPath,out newPath);
                    bestPath = newPath;

                    if (!ct.IsCancellationRequested) continue;
                    Console.WriteLine("Searched until depth " + currentDepth);
                    return bestPath.Peek();
                }

                Console.WriteLine("Searched until depth " + depth);
                return bestPath.Peek();




            }, ct);
            return searchTask;
        }

        private int searchTillDepth(BoardState boardState, int depth, int alpha, int beta,int side,
            ImmutableStack<Move> principleVariation,out ImmutableStack<Move> path)
        {
            path = ImmutableStack<Move>.Empty;

            if (depth == 0 || BoardState.IsStaleMate(boardState, player(side)) ||
                BoardState.IsCheckMate(boardState, player(side)))
            {
               
                return evaluation.GetValue(boardState) * side;
            }

            int bestValue = -INFINITY;
            
            var moves = BoardState.GetValidMoves(boardState, player(side));
            moves = principleVariation.IsEmpty ? orderMoves(moves) : orderMoves(moves, principleVariation.Peek());
          
            foreach (var move in moves)
            {
                var newState = BoardState.DoMove(boardState, move);
                ImmutableStack<Move> possiblePath;
                var tail = principleVariation.IsEmpty ? principleVariation : principleVariation.Pop();
                var possibleValue = - searchTillDepth(newState, depth - 1, -beta, -alpha, -side, tail,  out possiblePath);
                if (possibleValue > bestValue)
                {
                    bestValue = possibleValue;
                    path = possiblePath.Push(move);
                }
                alpha = Math.Max(alpha, possibleValue);
                if (alpha >= beta)
                    break;
            }
            
            return bestValue;

        }

        private IReadOnlyList<Move> orderMoves(IReadOnlyList<Move> moves, Move principleMove)
        {
            var sortedMoves = new List<Move>();
            sortedMoves.Add(principleMove);
            sortedMoves.AddRange(moves.Where(move => move.CapturedPiece != null));
            sortedMoves.AddRange(moves.Where(move => move.CapturedPiece == null));
            return sortedMoves;
        }
        private IReadOnlyList<Move> orderMoves(IReadOnlyList<Move> moves)
        {
            var sortedMoves = new List<Move>();
            sortedMoves.AddRange(moves.Where(move => move.CapturedPiece != null));
            sortedMoves.AddRange(moves.Where(move => move.CapturedPiece == null));
            return sortedMoves;
        }
    }
}
