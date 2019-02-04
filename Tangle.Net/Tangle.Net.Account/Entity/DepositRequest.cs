namespace Tangle.Net.Account.Entity
{
  using System;

  public abstract class DepositRequest
  {
    public long ExpectedAmount { get; set; }

    public bool MultiUse { get; set; }

    public DateTime TimeoutAt { get; set; }
  }
}