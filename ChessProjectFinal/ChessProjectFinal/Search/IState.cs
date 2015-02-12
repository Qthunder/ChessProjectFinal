using System.Collections.Generic;

namespace ChessProjectFinal.Search
{
  public interface IState
  {
      IList<IAction> GetActions();
      IState GetActionResult(IAction action);

  }
}
