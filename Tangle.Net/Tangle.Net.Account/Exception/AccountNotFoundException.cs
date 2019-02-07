namespace Tangle.Net.Account.Exception
{
  using System;

  public class AccountNotFoundException : Exception
  {
    public AccountNotFoundException(string accountId)
      : base($"Account with id {accountId} not found")
    {
    }
  }
}