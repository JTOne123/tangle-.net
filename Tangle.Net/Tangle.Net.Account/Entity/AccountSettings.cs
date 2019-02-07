namespace Tangle.Net.Account.Entity
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Services;
  using Tangle.Net.Account.Services.Memory;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository;

  public class AccountSettings
  {
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
      // TODO: Add default InputSelector
      return new AccountSettings
               {
                 Depth = 3,
                 MinimumWeightMagnitude = 14,
                 IotaRepository = iotaRepository,
                 Plugins = new List<IAccountPlugin>(),
                 SecurityLevel = Cryptography.SecurityLevel.Medium,
                 Store = new InMemoryAccountStore(),
                 SeedProvider = new InMemorySeedProvider(seed),
                 TimeSource = new UtcTimeSource()
               };
    }
  }
}