namespace Tangle.Net.Account.Services.Plugins
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Threading;
  using System.Threading.Tasks;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Account.Services.Events;
  using Tangle.Net.Entity;

  public class TransferPoller : IAccountPlugin
  {
    private const int PollingIntervalMilliseconds = 5000;

    private CancellationTokenSource taskTokenSource;

    /// <inheritdoc />
    public string Name => "TransferPoller";

    /// <inheritdoc />
    public void Shutdown()
    {
      this.taskTokenSource.Cancel();
    }

    /// <inheritdoc />
    public void Start(IAccount account)
    {
      this.taskTokenSource = new CancellationTokenSource();
      Task.Factory.StartNew(() => Poll(account), this.taskTokenSource.Token);
    }

    private static void CheckIncomingTransfers(
      IEnumerable<StoredDepositRequest> depositRequests,
      IEnumerable<PendingTransfer> pendingTransfers,
      IAccount account)
    {
      var depositAddresses = (from depositRequest in depositRequests
                              where depositRequest.TimeoutAt != default(DateTime)
                              select account.Settings.AddressGenerator.GetAddress(
                                account.Settings.SeedProvider.Seed,
                                depositRequest.SecurityLevel,
                                depositRequest.KeyIndex)).ToList();

      if (depositAddresses.Count == 0)
      {
        return;
      }

      var spentAddresses = new List<Address>();

      foreach (var transfer in pendingTransfers)
      {
        foreach (var transaction in transfer.Bundle.Transactions)
        {
          if (transaction.Value < 0)
          {
            spentAddresses.Add(transaction.Address);
          }
        }
      }

      // TODO: Get bundles for transactions
      var depositTransactions = account.Settings.IotaRepository.FindTransactionsByAddresses(depositAddresses);
      EventSource.Invoke(
        EventSource.EvenType.TransactionsReceived,
        account,
        new TransactionsReceivedEventArgs(depositTransactions.Hashes, depositAddresses, spentAddresses));
    }

    private static void CheckOutgoingTransfers(IEnumerable<PendingTransfer> pendingTransfers, IAccount account)
    {
      foreach (var transfer in pendingTransfers)
      {
        var inclusionStates = account.Settings.IotaRepository.GetLatestInclusion(new List<Hash> { transfer.Tail });
        var isIncluded = inclusionStates.States.First().Value;

        if (!isIncluded)
        {
          continue;
        }

        EventSource.Invoke(EventSource.EvenType.BundleConfirmed, account, new BundleConfirmedEventArgs(transfer.Bundle));
        account.Settings.Store.RemovePendingTransfers(account.Id, transfer.Tail);
      }
    }

    private static void Poll(IAccount account)
    {
      while (true)
      {
        var pendingTransfers = account.Settings.Store.GetPendingTransfers(account.Id);
        var depositRequests = account.Settings.Store.GetDepositRequests(account.Id);

        if (pendingTransfers.Count > 0)
        {
          CheckOutgoingTransfers(pendingTransfers, account);
        }

        if (depositRequests.Count > 0)
        {
          CheckIncomingTransfers(depositRequests, pendingTransfers, account);
        }

        Thread.Sleep(PollingIntervalMilliseconds);
      }
    }
  }
}