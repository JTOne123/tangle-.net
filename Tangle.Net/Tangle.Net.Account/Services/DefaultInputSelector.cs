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

      //var nodeInfo = account.Settings.IotaRepository.GetNodeInfo();
      //var solidSubTangleHash = new Hash(nodeInfo.LatestSolidSubtangleMilestone);

      var currentTime = account.Settings.TimeSource.Time;

      var primaryAddresses = new List<Address>();
      var primarySelection = new List<DepositRequest>();

      var secondaryAddresses = new List<Address>();
      var secondarySelection = new List<DepositRequest>();

      foreach (var depositRequest in depositRequests)
      {
        if (depositRequest.TimeoutAt == DateTime.MinValue)
        {
          var remainderAddress = account.Settings.AddressGenerator.GetAddress(
            account.Settings.SeedProvider.Seed,
            depositRequest.SecurityLevel,
            depositRequest.KeyIndex);

          primaryAddresses.Add(remainderAddress);
          primarySelection.Add(depositRequest);

          continue;
        }

        if (depositRequest.TimeoutAt.Ticks < currentTime.Ticks)
        {
          secondaryAddresses.Add(
            account.Settings.AddressGenerator.GetAddress(account.Settings.SeedProvider.Seed, depositRequest.SecurityLevel, depositRequest.KeyIndex));
          secondarySelection.Add(depositRequest);

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
          primarySelection.Add(depositRequest);

          continue;
        }

        primaryAddresses.Add(
          account.Settings.AddressGenerator.GetAddress(account.Settings.SeedProvider.Seed, depositRequest.SecurityLevel, depositRequest.KeyIndex));
        primarySelection.Add(depositRequest);
      }

      var balances = account.Settings.IotaRepository.GetBalances(primaryAddresses.Concat(secondaryAddresses).ToList());
      long balance = 0;

      var addressSelection = new List<Address>();
      for (var i = 0; i < primarySelection.Count; i++)
      {
        var depositRequest = primarySelection[i];
        var address = balances.Addresses[i];

        if (depositRequest.ExpectedAmount < address.Balance)
        {
          continue;
        }

        balance += address.Balance;
        if (address.Balance == 0 || balanceCheck)
        {
          continue;
        }

        addressSelection.Add(address);

        if (balance >= transferValue)
        {
          break;
        }
      }

      if (balance < transferValue || balanceCheck)
      {
        for (var i = primarySelection.Count; i < secondarySelection.Count + primarySelection.Count; i++)
        {
          var address = balances.Addresses[i];
          if (address.Balance == 0 && !balanceCheck)
          {
            // TODO acc.hasIncomingConsistentValueTransfer
          }

          balance += address.Balance;
          if (balanceCheck)
          {
            continue;
          }

          addressSelection.Add(address);
          if (balance >= transferValue)
          {
            break;
          }
        }
      }

      if (balance < transferValue)
      {
        throw new InsufficientBalanceException();
      }

      return new InputSelection(addressSelection);
    }
  }
}