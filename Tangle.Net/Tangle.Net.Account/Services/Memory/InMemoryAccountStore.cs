namespace Tangle.Net.Account.Services.Memory
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Account.Exception;
  using Tangle.Net.Entity;

  public class InMemoryAccountStore : IAccountStore
  {
    public InMemoryAccountStore()
    {
      this.AccountStates = new Dictionary<string, AccountState>();
    }

    private Dictionary<string, AccountState> AccountStates { get; }

    /// <inheritdoc />
    public void AddDepositRequest(string accountId, StoredDepositRequest depositRequest)
    {
      var account = this.GetAccountState(accountId);
      account.DepositRequests.Add(depositRequest);
    }

    /// <inheritdoc />
    public void AddPendingTransfer(string accountId, Hash tailHash, Bundle bundle, List<long> usedKeyIndices)
    {
      var account = this.GetAccountState(accountId);

      usedKeyIndices.ForEach(i => account.DepositRequests.RemoveAll(d => d.KeyIndex == i));
      account.PendingTransfers.Add(new PendingTransfer { Bundle = bundle, Tail = tailHash });
    }

    /// <inheritdoc />
    public void AddTailHash(string accountId, Hash originTailTransactionHash, Hash newTailTransactionHash)
    {
      throw new NotImplementedException();
    }

    /// <inheritdoc />
    public List<StoredDepositRequest> GetDepositRequests(string accountId)
    {
      var account = this.GetAccountState(accountId);

      var depositRequests = new StoredDepositRequest[account.DepositRequests.Count];
      account.DepositRequests.CopyTo(depositRequests);

      return depositRequests.ToList();
    }

    /// <inheritdoc />
    public List<PendingTransfer> GetPendingTransfers(string accountId)
    {
      var account = this.GetAccountState(accountId);

      var pendingTransfers = new PendingTransfer[account.PendingTransfers.Count];
      account.PendingTransfers.CopyTo(pendingTransfers);

      return pendingTransfers.ToList();
    }

    /// <inheritdoc />
    public AccountState LoadAccount(string accountId)
    {
      if (!this.AccountStates.ContainsKey(accountId))
      {
        var accountState = new AccountState();
        this.AccountStates.Add(accountId, accountState);

        return accountState;
      }

      var accountPair = this.AccountStates.FirstOrDefault(a => a.Key == accountId);
      return accountPair.Value;
    }

    /// <inheritdoc />
    public long ReadIndex(string accountId)
    {
      var account = this.GetAccountState(accountId);
      return account.LastUsedKeyIndex;
    }

    /// <inheritdoc />
    public void RemoveAccount(string accountId)
    {
      if (!this.AccountStates.ContainsKey(accountId))
      {
        return;
      }

      this.AccountStates.Remove(accountId);
    }

    /// <inheritdoc />
    public void RemoveDepositRequest(string accountId, long keyIndex)
    {
      var account = this.GetAccountState(accountId);
      account.DepositRequests.RemoveAll(d => d.KeyIndex == keyIndex);
    }

    /// <inheritdoc />
    public void RemovePendingTransfers(string accountId, Hash tailHash)
    {
      var account = this.GetAccountState(accountId);
      var transfer = account.PendingTransfers.FirstOrDefault(p => p.Tail.Value == tailHash.Value);

      if (transfer == null)
      {
        return;
      }

      account.PendingTransfers.Remove(transfer);
    }

    /// <inheritdoc />
    public void WriteIndex(string accountId, long newKeyIndex)
    {
      var account = this.GetAccountState(accountId);
      account.LastUsedKeyIndex = newKeyIndex;
    }

    private AccountState GetAccountState(string accountId)
    {
      if (!this.AccountStates.ContainsKey(accountId))
      {
        throw new AccountNotFoundException(accountId);
      }

      var accountPair = this.AccountStates.FirstOrDefault(a => a.Key == accountId);
      return accountPair.Value;
    }
  }
}