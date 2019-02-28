namespace Tangle.Net.Account.Entity
{
  using System;

  public class DepositRequest
  {
    public DepositRequest(DateTime timeoutAt, long? expectedAmount = 0, bool multiUse = false)
    {
      this.TimeoutAt = timeoutAt;
      this.ExpectedAmount = expectedAmount;
      this.MultiUse = multiUse;
    }

    public long? ExpectedAmount { get; }

    public bool MultiUse { get; }

    public DateTime TimeoutAt { get; }
  }
}