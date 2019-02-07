namespace Tangle.Net.Account.Services
{
  using Tangle.Net.Account.Entity;

  public interface IInputSelector
  {
    InputSelection SelectInputs(IAccount account, long transferValue, bool balanceCheck);
  }
}