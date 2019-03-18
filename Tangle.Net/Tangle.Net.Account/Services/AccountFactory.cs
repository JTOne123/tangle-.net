namespace Tangle.Net.Account.Services
{
  using Tangle.Net.Account.Entity;
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  public static class AccountFactory
  {
    public static Account Create(Seed seed, IIotaRepository repository)
    {
      var addressGenerator = new AddressGenerator();
      var accountId = addressGenerator.GetAddress(seed, SecurityLevel.Medium, 0);

      return new Account(accountId.Value, AccountSettings.GetDefault(seed, repository), addressGenerator);
    }
  }
}