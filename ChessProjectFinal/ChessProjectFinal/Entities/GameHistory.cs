using System.Collections.Generic;
using ChessProjectFinal.Model;

namespace ChessProjectFinal.Entities
{
   public class GameHistory
    {
       private readonly Stack<BoardState> history= new Stack<BoardState>();
       private int moves = 0;

       public BoardState CurrentState
       {
           get { return
           history.Peek(); } 
       }
       

       public int Moves
       {
           get { return moves; }
           private set { moves = value; }
       }

       public void Undo()
       {
           history.Pop();
           moves -= 1;

       }

       public void MakeMove(Move move)
       {
           history.Push(BoardState.DoMove(CurrentState,move));
           Moves++;
       }



       public GameHistory(BoardState boardState)
       {
           history.Push(boardState);
           Moves = 1;
       }

       public GameHistory() :this(BoardState.DefaultBoard())
       {
           
       }
    }
}
