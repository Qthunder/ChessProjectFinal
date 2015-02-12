namespace ChessProjectFinal.Search
{
   public class Node
   {
       public readonly IState State;
       public readonly IAction Action;

       public Node(IState state,IAction action)
       {
           this.State = state;
           this.Action = action;
       }

   }
}
