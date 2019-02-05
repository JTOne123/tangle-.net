namespace Tangle.Net.Account.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  public interface IAccountStore
  {
    void AddDepositRequest(string accountId, long keyIndex, StoredDepositRequest depositRequest);

    void AddPendingTransfer(string accountId, Hash tailTransactionHash, Bundle bundle, List<long> usedKeyIndices);

    void AddTailHash(string accountId, Hash originTailTransactionHash, Hash newTailTransactionHash);

    List<StoredDepositRequest> GetDepositRequests(string accountId);

    List<PendingTransfer> GetPendingTransfers(string accountId);

    AccountState LoadAccount(string accountId);

    long ReadIndex(string accountId);

    void RemoveAccount(string accountId);

    void RemoveDepositRequest(string accountId, long keyIndex);

    List<StoredDepositRequest> RemovePendingTransfers(string accountId, Hash tailTransactionHash);

    void WriteIndex(string accountId, long newKeyIndex);
  }
}