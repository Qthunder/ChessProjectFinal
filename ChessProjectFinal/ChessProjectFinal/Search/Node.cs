namespace ChessProjectFinal.Search
{
   public class Node
   {
       public readonly IState State;
       public readonly IAction Action;
       public readonly Node Parent;
 
   

       public Node(IState state,IAction action,Node parent)
       {
           State = state;
           Action = action;
           Parent = parent;
         
       }

   }
}
