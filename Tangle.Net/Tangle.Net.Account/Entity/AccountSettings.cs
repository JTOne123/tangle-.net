namespace Tangle.Net.Account.Entity
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Services;
  using Tangle.Net.Repository;

  public class AccountSettings
  {
    public int Depth { get; set; }

    public IIotaRepository IotaRepository { get; set; }

    public int MinimumWeightMagnitude { get; set; }

    public List<IAccountPlugin> Plugins { get; set; }

    public int SecurityLevel { get; set; }

    public ISeedProvider SeedProvider { get; set; }

    public IAccountStore Store { get; set; }

    public ITimeSource TimeSource { get; set; }
  }
}