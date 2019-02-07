namespace Tangle.Net.Account.Entity
{
  using Tangle.Net.Entity;

  public class PendingTransfer
  {
    public Bundle Bundle { get; set; }

    public Hash Tail { get; set; }
  }
}