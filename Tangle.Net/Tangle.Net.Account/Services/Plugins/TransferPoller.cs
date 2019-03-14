namespace Tangle.Net.Account.Services.Plugins
{
  using System.Collections.Generic;
  using System.Threading;
  using System.Threading.Tasks;

  using Tangle.Net.Account.Entity;

  public class TransferPoller : IAccountPlugin
  {
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

    private static void CheckIncomingTransfers(List<StoredDepositRequest> depositRequests, IAccount account)
    {
    }

    private static void CheckOutgoingTransfers(List<PendingTransfer> pendingTransfers, IAccount account)
    {
    }

    private static void Poll(IAccount account)
    {
      while (true)
      {
        var pendingTransfers = account.Settings.Store.GetPendingTransfers(account.Id);
        var depositRequests = account.Settings.Store.GetDepositRequests(account.Id);

        if (pendingTransfers.Count == 0 && depositRequests.Count == 0)
        {
          Thread.Sleep(5000);
          continue;
        }

        if (pendingTransfers.Count > 0)
        {
          CheckOutgoingTransfers(pendingTransfers, account);
        }

        if (depositRequests.Count > 0)
        {
          CheckIncomingTransfers(depositRequests, account);
        }
      }
    }
  }
}