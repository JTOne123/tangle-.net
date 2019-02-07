namespace Tangle.Net.Account.Entity
{
  using System;

  public class StoredDepositRequest : DepositRequest
  {
    /// <inheritdoc />
    public StoredDepositRequest(int keyIndex, int securityLevel, DateTime timeoutAt, long expectedAmount = 0, bool multiUse = false)
      : base(timeoutAt, expectedAmount, multiUse)
    {
      this.KeyIndex = keyIndex;
      this.SecurityLevel = securityLevel;
    }

    public int KeyIndex { get; }

    public int SecurityLevel { get; }
  }
}