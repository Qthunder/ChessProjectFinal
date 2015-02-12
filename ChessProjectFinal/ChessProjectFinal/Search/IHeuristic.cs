namespace ChessProjectFinal.Search
{
   public interface IHeuristic
   {
       int GetValue(IState state);
   }
}
