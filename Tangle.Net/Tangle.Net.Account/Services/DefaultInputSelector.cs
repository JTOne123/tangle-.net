namespace Tangle.Net.Account.Services
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

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

      foreach (var depositRequest in depositRequests)
      {
        if (depositRequest.TimeoutAt == DateTime.MinValue)
        {
          var remainderAddress = account.Settings.AddressGenerator.GetAddress(
            account.Settings.SeedProvider.Seed,
            depositRequest.SecurityLevel,
            depositRequest.KeyIndex);

          primaryAddresses.Add(remainderAddress);

          continue;
        }

        if (depositRequest.TimeoutAt.Ticks < currentTime.Ticks)
        {
          secondaryAddresses.Add(
            account.Settings.AddressGenerator.GetAddress(account.Settings.SeedProvider.Seed, depositRequest.SecurityLevel, depositRequest.KeyIndex));

          continue;
        }

        if (depositRequest.MultiUse)
        {
          if (depositRequest.ExpectedAmount == null)
          {
            continue;
          }

          primaryAddresses.Add(
            account.Settings.AddressGenerator.GetAddress(account.Settings.SeedProvider.Seed, depositRequest.SecurityLevel, depositRequest.KeyIndex));

          continue;
        }

        var address = account.Settings.AddressGenerator.GetAddress(
          account.Settings.SeedProvider.Seed,
          depositRequest.SecurityLevel,
          depositRequest.KeyIndex);

        primaryAddresses.Add(address);
      }

      var balances = account.Settings.IotaRepository.GetBalances(primaryAddresses.Concat(secondaryAddresses).ToList());
      var balance = balances.Addresses.Sum(a => a.Balance);

      return null;
    }
  }
}