namespace Tangle.Net.Account.Entity
{
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  public class PendingTransfer
  {
    public Bundle Bundle { get; set; }

    public List<Transaction> Tails { get; set; }
  }
}