﻿namespace Tangle.Net.Console
{
  using System;
  using System.Collections.Generic;

  using RestSharp;

  using Tangle.Net.Source.Entity;
  using Tangle.Net.Source.Repository;

  /// <summary>
  /// The program.
  /// </summary>
  internal static class Program
  {
    #region Methods

    /// <summary>
    /// The main.
    /// </summary>
    /// <param name="args">
    /// The args.
    /// </param>
    private static void Main(string[] args)
    {
      var repository = new RestIotaRepository(new RestClient("http://nelson1.iota.fm:80"));

      var transactions =
        repository.FindTransactionsByAddresses(
          new List<Address> { new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9") });

      var balances =
        repository.GetBalances(
          new List<Address>
            {
              new Address("GVZSJANZQULQICZFXJHHAFJTWEITWKQYJKU9TYFA9AFJLVIYOUCFQRYTLKRGCVY9KPOCCHK99TTKQGXA9"),
              new Address("HBBYKAKTILIPVUKFOTSLHGENPTXYBNKXZFQFR9VQFWNBMTQNRVOUKPVPRNBSZVVILMAFBKOTBLGLWLOHQ999999999")
            });

      var transactionsToApprove = repository.GetTransactionsToApprove();

      var nodeInfo = repository.GetNodeInfo();

      var neighbours = repository.GetNeighbors();

      Console.WriteLine("Done");
      Console.ReadKey();
    }

    #endregion
  }
}