namespace Tangle.Net.Account.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;
  using Tangle.Net.Repository.Client;

  public class Account : IAccount
  {
    public Account(string id, AccountSettings settings, IAddressGenerator addressGenerator)
    {
      this.Id = id;
      this.Settings = settings;
      this.AddressGenerator = addressGenerator;
    }

    /// <inheritdoc />
    public long AvailableBalance => this.Settings.InputSelector.SelectInputs(this, 0, true).InputValueSum;

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

    private IAddressGenerator AddressGenerator { get; }

    /// <inheritdoc />
    public Condition AllocateDepositRequest(DepositRequest request)
    {
      // TODO: request validation
      // TODO: account status validation (is running)
      var state = this.Settings.Store.LoadAccount(this.Id);

      var index = state.LastUsedKeyIndex + 1;
      var address = this.AddressGenerator.GetAddress(this.Settings.SeedProvider.Seed, this.Settings.SecurityLevel, index);

      this.Settings.Store.WriteIndex(this.Id, index);
      this.Settings.Store.AddDepositRequest(
        this.Id,
        new StoredDepositRequest(index, this.Settings.SecurityLevel, request.TimeoutAt, request.ExpectedAmount, request.MultiUse));

      return new Condition(address, request.TimeoutAt, request.ExpectedAmount, request.MultiUse);
    }

    /// <inheritdoc />
    public Bundle Send(List<Transfer> recipients)
    {
      // TODO: account status validation (is running)
      try
      {
        Address remainderAddress = null;
        List<Address> inputAddresses = null;
        var usedKeyIndices = new List<int>();

        var transferSum = recipients.Sum(r => r.ValueToTransfer);
        if (transferSum > 0)
        {
          var inputSelection = this.Settings.InputSelector.SelectInputs(this, transferSum, false);
          if (inputSelection.InputValueSum > transferSum)
          {
            var remainderCondition = this.AllocateDepositRequest(new DepositRequest(DateTime.MaxValue, inputSelection.InputValueSum - transferSum));
            remainderAddress = remainderCondition.Address;
          }

          inputAddresses = inputSelection.InputAddresses;
          usedKeyIndices = inputSelection.KeyIndices;
        }

        var bundle = new Bundle();
        recipients.ForEach(r => bundle.AddTransfer(r));

        // TODO: Emit PrepareTransfer event
        this.Settings.IotaRepository.PrepareTransfer(
          this.Settings.SeedProvider.Seed,
          bundle,
          this.Settings.SecurityLevel,
          remainderAddress,
          inputAddresses);

        // TODO: Emit GettingTransactionsToApprove event
        var tips = this.Settings.IotaRepository.GetTransactionsToApprove(this.Settings.Depth);

        // TODO: Emit AttachToTangle event
        var transactionTrytes = this.Settings.IotaRepository.AttachToTangle(
          tips.BranchTransaction,
          tips.TrunkTransaction,
          bundle.Transactions,
          this.Settings.MinimumWeightMagnitude);

        // TODO: change storage to transactionTrytes
        var bundleToSend = Bundle.FromTransactionTrytes(transactionTrytes);
        this.Settings.Store.AddPendingTransfer(this.Id, bundleToSend.TailTransaction.Hash, bundleToSend, usedKeyIndices);

        this.Settings.IotaRepository.BroadcastAndStoreTransactions(transactionTrytes);

        // TODO: Emit SentTransfer event
        return bundle;
      }
      catch (Exception exception)
      {
        // TODO: ensure that the allocated remainder address is deleted from the store if the send operation wasn't successful.
        throw;
      }
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