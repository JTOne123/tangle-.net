namespace Tangle.Net.Account.Entity
{
  using System;
  using System.Web;

  using Tangle.Net.Entity;

  public class Condition : DepositRequest
  {
    public Condition(Address address, DateTime timeoutAt)
    {
      this.Address = address;
      this.TimeoutAt = timeoutAt;
    }

    public Address Address { get; }

    public static Condition FromMagnetLink(Uri uri)
    {
      if (uri.Scheme != "iota")
      {
        throw new Exception($"Invalid Uri scheme {uri.Scheme}. Only scheme accepted is 'iota://'");
      }

      if (uri.Host.Length != 90)
      {
        throw new Exception("Invalid address length. Please provide the address with checksum (length 90 trytes)");
      }

      var queryParameters = HttpUtility.ParseQueryString(uri.Query);

      if (string.IsNullOrEmpty(queryParameters.Get("t")) || string.IsNullOrEmpty(queryParameters.Get("m"))
                                                         || string.IsNullOrEmpty(queryParameters.Get("am")))
      {
        throw new Exception("Invalid query parameters. Please provide t (timeoutAt), m (MultiUse) and am (ExpectedAmount)");
      }

      return new Condition(new Address(uri.Host.ToUpper()), UnixTimeStampToDateTime(int.Parse(queryParameters.Get("t"))))
               {
                 ExpectedAmount = long.Parse(queryParameters.Get("am")), MultiUse = bool.Parse(queryParameters.Get("m"))
               };
    }

    public string ToMagnetLink()
    {
      return
        $"iota://{this.Address.WithChecksum().Value}{this.Address.Checksum.Value}/?t={DateTimeToUnixTimeStamp(this.TimeoutAt)}&m={this.MultiUse.ToString().ToLower()}&am={this.ExpectedAmount}";
    }

    public Transfer ToTransfer()
    {
      return new Transfer { Address = this.Address, ValueToTransfer = this.ExpectedAmount };
    }

    private static int DateTimeToUnixTimeStamp(DateTime date)
    {
      return (int)date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
    }

    private static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
    {
      return new DateTime(1970, 1, 1).AddSeconds(unixTimeStamp);
    }
  }
}