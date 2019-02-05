namespace Tangle.Net.Account.Entity
{
  using System;

  using Tangle.Net.Entity;

  public class Condition : DepositRequest
  {
    public Condition(Address address, DateTime timeoutAt)
    {
      this.Address = address;
      this.TimeoutAt = timeoutAt;
    }

    public Address Address { get; set; }

    public string ToMagnetLink()
    {
      var unixTimestamp = (int)this.TimeoutAt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
      return
        $"iota://{this.Address.WithChecksum().Value}{this.Address.Checksum.Value}/?t={unixTimestamp}&m={this.MultiUse.ToString().ToLower()}&am={this.ExpectedAmount}";
    }
  }
}