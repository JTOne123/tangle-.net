namespace Tangle.Net.Account.Entity
{
  using Tangle.Net.Entity;

  public class Condition : DepositRequest
  {
    public Address Address { get; set; }
  }
}