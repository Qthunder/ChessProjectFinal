using System.Collections.Generic;

namespace ChessProjectFinal.Search
{
  public interface IState
  {
      IReadOnlyList<IAction> GetActions();
      IState GetActionResult(IAction action);

  }
}
