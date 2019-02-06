using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tangle.Net.Account.Services.Memory
{
  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  public class InMemoryAccount : IAccount
  {
    /// <inheritdoc />
    public string Id { get; }

    /// <inheritdoc />
    public long AvailableBalance { get; }

    /// <inheritdoc />
    public long TotalBalance { get; }

    /// <inheritdoc />
    public bool IsNew { get; }

    /// <inheritdoc />
    public void Start()
    {
    }

    /// <inheritdoc />
    public void Shutdown()
    {
    }

    /// <inheritdoc />
    public void Send(List<Transfer> recipients)
    {
    }

    /// <inheritdoc />
    public Condition AllocateDepositRequest(string accountId)
    {
      return null;
    }

    /// <inheritdoc />
    public void UpdateSettings(AccountSettings accountSettings)
    {
    }
  }
}
