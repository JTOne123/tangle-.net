namespace Tangle.Net.Account.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  public interface IAccountStore
  {
    void AddDepositRequest(string accountId, StoredDepositRequest depositRequest);

    void AddPendingTransfer(string accountId, Hash tailHash, Bundle bundle, List<int> usedKeyIndices);

    void AddTailHash(string accountId, Hash originTailHash, Hash newTailHash);

    List<StoredDepositRequest> GetDepositRequests(string accountId);

    List<PendingTransfer> GetPendingTransfers(string accountId);

    AccountState LoadAccount(string accountId);

    long ReadIndex(string accountId);

    void RemoveAccount(string accountId);

    void RemoveDepositRequest(string accountId, int keyIndex);

    void RemovePendingTransfers(string accountId, Hash tailHash);

    void WriteIndex(string accountId, int newKeyIndex);
  }
}