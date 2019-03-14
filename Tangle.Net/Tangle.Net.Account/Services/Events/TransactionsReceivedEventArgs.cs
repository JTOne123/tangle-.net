namespace Tangle.Net.Account.Services.Events
{
  using System;
  using System.Collections.Generic;

  using Tangle.Net.Entity;

  public class TransactionsReceivedEventArgs : EventArgs
  {
    public TransactionsReceivedEventArgs(List<Hash> transactionHashes, List<Address> depositAddresses, List<Address> spentAddresses)
    {
      this.TransactionHashes = transactionHashes;
      this.DepositAddresses = depositAddresses;
      this.SpentAddresses = spentAddresses;
    }

    public List<Address> DepositAddresses { get; }

    public List<Address> SpentAddresses { get; }

    public List<Hash> TransactionHashes { get; }
  }
}