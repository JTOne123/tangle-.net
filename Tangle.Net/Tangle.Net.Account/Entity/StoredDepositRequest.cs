namespace Tangle.Net.Account.Entity
{
  public class StoredDepositRequest : DepositRequest
  {
    public long KeyIndex { get; set; }

    public int SecurityLevel { get; set; }
  }
}