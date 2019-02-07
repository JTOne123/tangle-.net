namespace Tangle.Net.Account.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  public interface IAccount
  {
    /// <summary>
    /// Runs the input selection with the stored CDRs in order to determine the available balance for funding transfers.
    /// </summary>
    long AvailableBalance { get; }

    /// <summary>
    /// Returns the account’s unique identifier which is a sha256 hash of the account’s seed
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Checks whether the state of the account is new.
    /// An account is considered new if no pending transfers or CDRs are stored.
    /// </summary>
    bool IsNew { get; }

    bool IsRunning { get; set; }

    AccountSettings Settings { get; set; }

    /// <summary>
    /// Uses all stored CDRs to determine the current total balance.
    /// </summary>
    long TotalBalance { get; }

    /// <summary>
    /// Allocates a new CDR and increments the latest used key index in the store.
    /// </summary>
    /// <param name="request">
    /// The deposit request.
    /// </param>
    /// <returns>
    /// Returns an object describing the conditions around the deposit address.
    /// </returns>
    Condition AllocateDepositRequest(DepositRequest request);

    /// <summary>
    /// Runs input selection, allocates a special CDR for the remainder address if needed,
    /// creates a new bundle, atomically stores it, removes all used CDRs from the store,
    /// broadcasts the bundle and returns it.
    /// </summary>
    /// <param name="recipients">
    /// Recipient is a bundle.Transfer but with a nicer name.
    /// </param>
    void Send(List<Transfer> recipients);

    /// <summary>
    /// Shuts down all plugins and flags the account as “non-running”.
    /// Executing any method call after Shutdown() will return an error.
    /// </summary>
    void Shutdown();

    /// <summary>
    /// Loads the initial account state and starts all given plugins. Also flags the account as “running”.
    /// Executing any other methods before calling Start() will return an error. 
    /// </summary>
    void Start();

    void UpdateSettings(AccountSettings accountSettings);
  }
}