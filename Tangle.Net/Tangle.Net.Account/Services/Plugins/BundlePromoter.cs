namespace Tangle.Net.Account.Services.Plugins
{
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  public class BundlePromoter : IAccountPlugin
  {
    private const int PromotionIntervalMilliseconds = 20000;

    private CancellationTokenSource taskTokenSource;

    /// <inheritdoc />
    public string Name => "BundlePromoter";

    /// <inheritdoc />
    public void Shutdown()
    {
      this.taskTokenSource.Cancel();
    }

    /// <inheritdoc />
    public void Start(IAccount account)
    {
      this.taskTokenSource = new CancellationTokenSource();
      Task.Factory.StartNew(() => Promote(account), this.taskTokenSource.Token);
    }

    private static void Promote(IAccount account)
    {
      while (true)
      {
        var pendingTransfers = account.Settings.Store.GetPendingTransfers(account.Id);

        foreach (var transfer in pendingTransfers)
        {
          try
          {
            account.Settings.IotaRepository.PromoteTransactionAsync(transfer.Tail, account.Settings.Depth, account.Settings.MinimumWeightMagnitude).Wait();
          }
          catch (ArgumentException)
          {
          }
        }

        Thread.Sleep(PromotionIntervalMilliseconds);
      }
    }
  }
}