namespace Tangle.Net.Account.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Account.Exception;
  using Tangle.Net.Entity;

  public class DefaultInputSelector : IInputSelector
  {
    /// <inheritdoc />
    public InputSelection SelectInputs(IAccount account, long transferValue, bool balanceCheck)
    {
      var depositRequests = account.Settings.Store.GetDepositRequests(account.Id);

      if (depositRequests.Count == 0)
      {
        if (balanceCheck)
        {
          return new InputSelection(new List<Address>());
        }

        throw new InsufficientBalanceException();
      }

      var nodeInfo = account.Settings.IotaRepository.GetNodeInfo();
      var solidSubTangleHash = new Hash(nodeInfo.LatestSolidSubtangleMilestone);
      var currentTime = account.Settings.TimeSource.Time;

      var primaryAddresses = new List<Address>();
      var secondaryAddresses = new List<Address>();

      return null;
    }
  }
}