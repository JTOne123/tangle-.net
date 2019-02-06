namespace Tangle.Net.Account.Services
{
  using Tangle.Net.Entity;

  public interface ISeedProvider
  {
    Seed Seed { get; }
  }
}