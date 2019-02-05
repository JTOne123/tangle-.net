namespace Tangle.Net.Account.Entity
{
  using System.Collections.Generic;

  public class AccountState
  {
    public List<Condition> DepositRequests { get; set; }

    public long LastUsedKeyIndex { get; set; }

    public List<PendingTransfer> PendingTransfers { get; set; }
  }
}