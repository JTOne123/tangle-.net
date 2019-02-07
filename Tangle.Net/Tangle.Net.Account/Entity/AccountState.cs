namespace Tangle.Net.Account.Entity
{
  using System.Collections.Generic;

  public class AccountState
  {
    public AccountState()
    {
      this.DepositRequests = new List<StoredDepositRequest>();
      this.PendingTransfers = new List<PendingTransfer>();
    }

    public List<StoredDepositRequest> DepositRequests { get; set; }

    public long LastUsedKeyIndex { get; set; }

    public List<PendingTransfer> PendingTransfers { get; set; }
  }
}