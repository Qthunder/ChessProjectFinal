using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ChessProjectFinal.Entities;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.ChessSearch
{
    public class NegaMaxSearchChess
    {
        #region CONSTANTS
        private static Player player(int side)
        {
            return (side == 1) ? Player.WHITE : Player.BLACK;
        }
        private const int INFINITY = 10000;
        #endregion
        #region SETTINGS

        private bool usingPrincipleVariation=true;
        private int searchDepth=4;
        private int searchTime=30;

        public void NewSettings(bool usingPrincipleVariation, int searchDepth, int searchTime)
        {
            this.searchTime=searchTime;
            this.searchDepth = searchDepth;
            this.usingPrincipleVariation = usingPrincipleVariation;
        }
        #endregion

        

        private readonly EvaluationFunction evaluation;

        #region PRINCIPLE VARIATION
        public Move[] PrincipleVariation;
        private int moveIndex;
        private bool onPrincipleVariation;
        #endregion
        #region THE SEARCH FUNCTION
        private int searchTillDepth(BoardState boardState, int depth, int alpha, int beta, int side, out ImmutableStack<Move> path,CancellationToken ct)
        {   
            path = ImmutableStack<Move>.Empty;
            if (depth == 0 || BoardState.IsStaleMate(boardState, player(side)) ||
                BoardState.IsCheckMate(boardState, player(side)))
            {
                onPrincipleVariation = false;
                return evaluation.GetValue(boardState);
            }

          

            var moves = orderMoves( new List<Move>(BoardState.GetValidMoves(boardState, player(side))));
            
          
               
           
            var bestValue = -side*INFINITY;
            foreach (var move in moves)
            {


                var newState = BoardState.DoMove(boardState, move);
                ImmutableStack<Move> possiblePath;
                var possibleValue = searchTillDepth(newState, depth - 1, beta, alpha, -side,  out possiblePath);
                if (possibleValue * side > bestValue *side)
                {
                    bestValue = possibleValue;
                    path = possiblePath.Push(move);
                }

                alpha = side == 1 ? Math.Max(alpha, bestValue) : Math.Min(alpha,bestValue);
                if (alpha >= beta && side==1)
                    break;
                if (alpha <= beta && side == -1)
                    break;
            }

            return bestValue;

        }
        #endregion
       


        public NegaMaxSearchChess(EvaluationFunction evaluation)
        {
            this.evaluation = evaluation;
            usingPrincipleVariation = true;

        }
        public Task<Move> Search(Player player, BoardState boardstate,)
        {
            
            var timeout = TimeSpan.FromSeconds(searchTime);
            var cts = new CancellationTokenSource(timeout);
            var ct = cts.Token;
            return iterativeDeapeningSearch(player, boardstate, ct);

        }
        private Task<Move> iterativeDeapeningSearch(Player player, BoardState boardState, CancellationToken ct)
        {
            onPrincipleVariation = false;
            var side = player == Player.WHITE ? 1 : -1;
            var searchTask = Task.Factory.StartNew(() =>
            {
                var bestPath = ImmutableStack<Move>.Empty;
                for (var currentDepth = 1; currentDepth < searchDepth; currentDepth++)
                {   
                    ImmutableStack<Move> newPath;
                    searchTillDepth(boardState, currentDepth, -INFINITY*side, INFINITY*side, side,out newPath,ct);
                    if (usingPrincipleVariation)
                    {
                        onPrincipleVariation = true;
                        moveIndex = 0;
                        PrincipleVariation=new Move[newPath.Count()];
                        foreach (var move in newPath)
                            PrincipleVariation[moveIndex++] = move;
                        moveIndex = 0;

                    }
                    if (!newPath.IsEmpty)
                        bestPath = newPath;

                    if (!ct.IsCancellationRequested ) continue;
                    Console.WriteLine("Searched until depth " + currentDepth);
                    return bestPath.Peek();
                }

                Console.WriteLine("Searched until depth " + searchDepth);
                return bestPath.Peek();




            }, ct);
            return searchTask;
        }

       

       
        private IEnumerable<Move> orderMoves(IReadOnlyList<Move> moves)
        {
            var sortedMoves = new List<Move>();
            if (onPrincipleVariation && moveIndex<PrincipleVariation.Count())
            {
                sortedMoves.AddRange(moves.Where(move=> move.Equals(PrincipleVariation[moveIndex])));
                moveIndex++;
            }
          
            sortedMoves.AddRange(moves.Where(move => move.CapturedPiece != null));
            sortedMoves.AddRange(moves.Where(move => move.CapturedPiece == null));
            return sortedMoves;
        }
    }
}
