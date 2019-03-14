namespace Tangle.Net.Account.Entity
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Services;
  using Tangle.Net.Account.Services.Memory;
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  public class AccountSettings
  {
    public IAddressGenerator AddressGenerator { get; set; }

    public int Depth { get; set; }

    public IInputSelector InputSelector { get; set; }

    public IIotaRepository IotaRepository { get; set; }

    public int MinimumWeightMagnitude { get; set; }

    public List<IAccountPlugin> Plugins { get; set; }

    public int SecurityLevel { get; set; }

    public ISeedProvider SeedProvider { get; set; }

    public IAccountStore Store { get; set; }

    public ITimeSource TimeSource { get; set; }

    public static AccountSettings GetDefault(Seed seed, IIotaRepository iotaRepository)
    {
      return new AccountSettings
               {
                 Depth = 3,
                 MinimumWeightMagnitude = 14,
                 IotaRepository = iotaRepository,
                 Plugins = new List<IAccountPlugin>(),
                 SecurityLevel = Cryptography.SecurityLevel.Medium,
                 Store = new InMemoryAccountStore(),
                 SeedProvider = new InMemorySeedProvider(seed),
                 TimeSource = new UtcTimeSource(),
                 AddressGenerator = new AddressGenerator(),
                 InputSelector = new DefaultInputSelector()
               };
    }
  }
}