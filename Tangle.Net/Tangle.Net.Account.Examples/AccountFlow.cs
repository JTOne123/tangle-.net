namespace Tangle.Net.Account.Examples
{
  using System;
  using System.Threading;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Account.Services;
  using Tangle.Net.Account.Services.Events;
  using Tangle.Net.Entity;
  using Tangle.Net.Examples;

  public class AccountFlow
  {
    public void Execute()
    {
      EventSource.TransactionsReceived += (sender, args) =>
        {
          ((TransactionsReceivedEventArgs)args).TransactionHashes.ForEach(h => Console.WriteLine(h.Value));
        };

      var account = AccountFactory.Create(Seed.Random(), Utils.Repository);
      account.Start();

      var condition = account.AllocateDepositRequest(new DepositRequest(DateTime.UtcNow.AddDays(1), 10));

      while (true)
      {
        Thread.Sleep(1000);
      }
    }
  }
}