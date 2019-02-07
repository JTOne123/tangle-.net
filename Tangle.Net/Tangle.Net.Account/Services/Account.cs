namespace Tangle.Net.Account.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  public class Account : IAccount
  {
    public Account(string id, AccountSettings settings)
    {
      this.Id = id;
      this.Settings = settings;
    }

    /// <inheritdoc />
    public long AvailableBalance { get; }

    /// <inheritdoc />
    public string Id { get; }

    /// <inheritdoc />
    public bool IsNew
    {
      get
      {
        var accountState = this.Settings.Store.LoadAccount(this.Id);
        return accountState.PendingTransfers.Count == 0;
      }
    }

    /// <inheritdoc />
    public bool IsRunning { get; set; }

    /// <inheritdoc />
    public AccountSettings Settings { get; set; }

    /// <inheritdoc />
    public long TotalBalance { get; }

    /// <inheritdoc />
    public Condition AllocateDepositRequest(string accountId)
    {
      return null;
    }

    /// <inheritdoc />
    public void Send(List<Transfer> recipients)
    {
    }

    /// <inheritdoc />
    public void Shutdown()
    {
      this.ShutdownPlugins();
      this.IsRunning = false;
    }

    /// <inheritdoc />
    public void Start()
    {
      this.StartPlugins();
      this.IsRunning = true;
    }

    /// <inheritdoc />
    public void UpdateSettings(AccountSettings accountSettings)
    {
      this.ShutdownPlugins();
      this.Settings = accountSettings;
      this.StartPlugins();
    }

    private void ShutdownPlugins()
    {
      foreach (var plugin in this.Settings.Plugins)
      {
        plugin.Shutdown();
      }
    }

    private void StartPlugins()
    {
      foreach (var plugin in this.Settings.Plugins)
      {
        plugin.Start(this);
      }
    }
  }
}